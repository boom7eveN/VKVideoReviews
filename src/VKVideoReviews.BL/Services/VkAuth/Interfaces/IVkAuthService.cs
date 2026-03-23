using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Services.VkAuth.Interfaces;

public interface IVkAuthService
{
    string BuildAuthorizationUrl();
    Task<VkTokensApiResponse> ExchangeCodeForTokenAsync(VkAuthCallbackModel vkAuthCallbackModel);
}