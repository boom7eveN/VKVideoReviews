using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Services.AppAuth.Interfaces;

public interface IAppAuthService
{
    Task<AuthTokensResult> SignInWithVkTokensAsync(VkTokensApiResponse vkTokens);
    Task<AuthTokensResult?> RefreshAsync(string refreshToken);
}