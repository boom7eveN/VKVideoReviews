using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Requests;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.BL.Services.AppAuth;

public class AppAuthService(
    IVkApiMethodsClient httpClient,
    IMapper mapper,
    IUsersRepository usersRepository,
    IUserTokensRepository userTokensRepository,
    IUserAppSessionsRepository appSessionsRepository,
    JwtAuthSettings settings,
    IJwtTokenService jwtTokenService,
    long[] adminVkUserIds)
    : IAppAuthService
{
    private static readonly TimeSpan VkRefreshTokenLifeTime = TimeSpan.FromDays(180);

    public async Task<AuthTokensModel> SignInWithVkTokensAsync(VkTokensApiResponse vkTokens)
    {
        var userResponse = await httpClient.GetUserAsync(new VkUserGetRequest
        {
            UserId = vkTokens.UserId,
            AccessToken = vkTokens.AccessToken,
        });

        var user = await usersRepository.GetByVkUserIdAsync(userResponse.VkUserId);

        if (user == null)
        {
            user = mapper.Map<UserEntity>(userResponse);
            user.UserId = Guid.NewGuid();
            user = await usersRepository.AddAsync(user);
        }
        else
        {
            mapper.Map(userResponse, user);
            await usersRepository.UpdateAsync(user);
        }

        if (adminVkUserIds.Length > 0
            && adminVkUserIds.Contains(user!.VkUserId)
            && !user.IsAdmin)
        {
            user.IsAdmin = true;
            await usersRepository.UpdateAsync(user);
        }

        var accessExpiresAt = DateTime.UtcNow.AddSeconds(vkTokens.ExpiresIn);
        var refreshExpiresAt = DateTime.UtcNow.AddDays(VkRefreshTokenLifeTime.Days);

        var userToken = new UserTokenEntity()
        {
            UserId = user!.UserId,
            TokenRecordId = Guid.NewGuid(),
            VkUserId = vkTokens.UserId,
            AccessToken = vkTokens.AccessToken,
            RefreshToken = vkTokens.RefreshToken,
            AccessTokenExpiresAt = accessExpiresAt,
            RefreshTokenExpiresAt = refreshExpiresAt,
            CreatedAt = DateTime.UtcNow,
        };

        await userTokensRepository.UpsertForUserAsync(userToken);
        await appSessionsRepository.RemoveAllForUserAsync(user.UserId);

        var refreshToken = CreateRefreshToken();
        var newSession = new UserAppSessionEntity
        {
            SessionId = Guid.NewGuid(),
            UserId = user.UserId,
            RefreshTokenHash = Hash(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(settings.RefreshTokenLifeTimeDays),
            CreatedAt = DateTime.UtcNow,
        };
        await appSessionsRepository.AddAsync(newSession);
        return new AuthTokensModel()
        {
            AccessToken = jwtTokenService.CreateAccessToken(user),
            RefreshToken = refreshToken,
            ExpiresInSeconds = jwtTokenService.GetAccessTokenLifetimeSeconds(),
            TokenType = "Bearer"
        };
    }

    public async Task<AuthTokensModel?> RefreshAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var refreshTokenHash = Hash(refreshToken);
        var session = await appSessionsRepository.GetByRefreshTokenHashAsync(refreshTokenHash);
        if (session == null || session.ExpiresAt <= DateTime.UtcNow)
            return null;

        var user = session.User;

        await appSessionsRepository.RemoveAsync(session);

        var newRefreshToken = CreateRefreshToken();
        var newSession = new UserAppSessionEntity
        {
            SessionId = Guid.NewGuid(),
            UserId = user.UserId,
            RefreshTokenHash = Hash(newRefreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(settings.RefreshTokenLifeTimeDays),
            CreatedAt = DateTime.UtcNow,
        };
        await appSessionsRepository.AddAsync(newSession);

        return new AuthTokensModel
        {
            AccessToken = jwtTokenService.CreateAccessToken(user),
            RefreshToken = newRefreshToken,
            ExpiresInSeconds = jwtTokenService.GetAccessTokenLifetimeSeconds(),
            TokenType = "Bearer"
        };
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

    private string GetParametersForUserGet(VkTokensApiResponse vkTokens)
    {
        var requestParams = new Dictionary<string, string>()
        {
            { "user_id", vkTokens.UserId.ToString() },
            { "access_token", vkTokens.AccessToken },
            { "fields", "photo_200" },
            { "v", "5.131" }
        };
        var queryString = string.Join("&", requestParams
            .Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        return queryString;
    }
}