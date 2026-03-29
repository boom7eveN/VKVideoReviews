using VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;
using VKVideoReviews.BL.Services.AppAuth.Models;

namespace VKVideoReviews.BL.Services.AppAuth.Interfaces;

public interface IAppAuthService
{
    Task<AuthTokensResult> SignInWithVkTokensAsync(VkTokensApiResponse vkTokens);
    Task<AuthTokensResult?> RefreshAsync(string refreshToken);
}