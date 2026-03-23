using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Clients.Interfaces;

public interface IVkIdClient
{
    Task<VkTokensApiResponse> GetUserTokensAsync(FormUrlEncodedContent content);
}