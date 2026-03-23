using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Clients.Interfaces;

public interface IVkApiAuthClient
{
    Task<VkTokensApiResponse> GetUserTokensAsync(FormUrlEncodedContent content);
}