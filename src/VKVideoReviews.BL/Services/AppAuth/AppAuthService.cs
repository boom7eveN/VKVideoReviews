using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.BL.Services.VkAuth.Models;
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
    IJwtTokenService jwtTokenService)
    : IAppAuthService
{
    private static readonly TimeSpan VkRefreshTokenLifeTime = TimeSpan.FromDays(180);

    public async Task<AuthTokensResult> SignInWithVkTokensAsync(VkTokensApiResponse vkTokens)
    {
        var userGetParams = GetParametersForUserGet(vkTokens);
        var userResponse = await httpClient.GetUserAsync(userGetParams);

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
        return new AuthTokensResult()
        {
            AccessToken = jwtTokenService.CreateAccessToken(user),
            RefreshToken = refreshToken,
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
            { "access_token", $"{vkTokens.AccessToken}" },
            { "fields", "photo_200" },
        };
        var queryString = string.Join("&", requestParams
            .Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        return queryString;
    }
}