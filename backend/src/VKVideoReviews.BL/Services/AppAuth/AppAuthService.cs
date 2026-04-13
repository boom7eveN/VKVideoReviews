using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Requests;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.AppAuth;

public class AppAuthService(
    IVkApiMethodsClient httpClient,
    IMapper mapper,
    IAuthUnitOfWork unitOfWork,
    JwtAuthSettings settings,
    IJwtTokenService jwtTokenService,
    ITokenEncryptionService tokenEncryptionService,
    long[] adminVkUserIds)
    : IAppAuthService
{
    private static readonly TimeSpan VkRefreshTokenLifeTime = TimeSpan.FromDays(180);

    public async Task<AuthTokensModel> SignInWithVkTokensAsync(VkTokensApiResponse vkTokens)
    {
        var userResponse = await httpClient.GetUserAsync(new VkUserGetRequest
        {
            UserId = vkTokens.UserId,
            AccessToken = vkTokens.AccessToken
        });

        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var user = await unitOfWork.Users.GetUserByVkIdAsync(userResponse.VkUserId);

            if (user == null)
            {
                user = mapper.Map<UserEntity>(userResponse);
                user.UserId = Guid.NewGuid();
                user = await unitOfWork.Users.CreateUserAsync(user);
            }
            else
            {
                mapper.Map(userResponse, user);
                unitOfWork.Users.UpdateUserAsync(user);
            }

            await unitOfWork.SaveChangesAsync();

            if (adminVkUserIds.Length > 0
                && adminVkUserIds.Contains(user!.VkUserId)
                && !user.IsAdmin)
            {
                user.IsAdmin = true;
                unitOfWork.Users.UpdateUserAsync(user);
            }

            var accessExpiresAt = DateTime.UtcNow.AddSeconds(vkTokens.ExpiresIn);
            var refreshExpiresAt = DateTime.UtcNow.AddDays(VkRefreshTokenLifeTime.Days);

            var userToken = new UserTokenEntity
            {
                UserId = user!.UserId,
                TokenRecordId = Guid.NewGuid(),
                VkUserId = vkTokens.UserId,
                VkAccessTokenEncrypted = tokenEncryptionService.Encrypt(vkTokens.AccessToken),
                VkRefreshTokenEncrypted = tokenEncryptionService.Encrypt(vkTokens.RefreshToken),
                AccessTokenExpiresAt = accessExpiresAt,
                RefreshTokenExpiresAt = refreshExpiresAt,
                CreatedAt = DateTime.UtcNow
            };

            await unitOfWork.UserTokens.UpsertTokensForUserAsync(userToken);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.UserAppSessions.DeleteAllUserSessionsForUserAsync(user.UserId);
            await unitOfWork.SaveChangesAsync();

            var refreshToken = CreateRefreshToken();
            var newSession = new UserAppSessionEntity
            {
                SessionId = Guid.NewGuid(),
                UserId = user.UserId,
                AppRefreshTokenHash = Hash(refreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(settings.RefreshTokenLifeTimeDays),
                RefreshTokenCreatedAt = DateTime.UtcNow
            };
            await unitOfWork.UserAppSessions.CreateUserSessionAsync(newSession);
            await unitOfWork.CommitAsync();
            return new AuthTokensModel
            {
                AccessToken = jwtTokenService.CreateAccessToken(user),
                RefreshToken = refreshToken,
                ExpiresInSeconds = jwtTokenService.GetAccessTokenLifetimeSeconds(),
                TokenType = "Bearer"
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<AuthTokensModel?> RefreshAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var refreshTokenHash = Hash(refreshToken);
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var session = await unitOfWork.UserAppSessions.GetUserSessionByRefreshTokenHashAsync(refreshTokenHash);
            if (session == null || session.RefreshTokenExpiresAt <= DateTime.UtcNow)
            {
                await unitOfWork.RollbackAsync();
                return null;
            }

            var user = session.User;

            unitOfWork.UserAppSessions.DeleteUserSession(session);
            await unitOfWork.SaveChangesAsync();

            var newRefreshToken = CreateRefreshToken();
            var newSession = new UserAppSessionEntity
            {
                SessionId = Guid.NewGuid(),
                UserId = user.UserId,
                AppRefreshTokenHash = Hash(newRefreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(settings.RefreshTokenLifeTimeDays),
                RefreshTokenCreatedAt = DateTime.UtcNow
            };
            await unitOfWork.UserAppSessions.CreateUserSessionAsync(newSession);
            await unitOfWork.CommitAsync();
            return new AuthTokensModel
            {
                AccessToken = jwtTokenService.CreateAccessToken(user),
                RefreshToken = newRefreshToken,
                ExpiresInSeconds = jwtTokenService.GetAccessTokenLifetimeSeconds(),
                TokenType = "Bearer"
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    private static string CreateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(48);
        return Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }

    private static string Hash(string refreshToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
        return Convert.ToBase64String(bytes);
    }
}