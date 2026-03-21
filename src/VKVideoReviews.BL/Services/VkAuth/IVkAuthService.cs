using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Services.VkAuth;

public interface IVkAuthService
{
    PckeData GeneratePkce();
    string BuildAuthorizationUrl(PckeData data, string state);
}