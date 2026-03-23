using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Services.VkAuth;

public interface IVkAuthService
{
    string BuildAuthorizationUrl();
    Task<string> ProcessCallback(VkAuthCallbackModel vkAuthCallbackModel);
}