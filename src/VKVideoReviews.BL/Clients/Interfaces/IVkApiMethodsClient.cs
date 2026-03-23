using VKVideoReviews.BL.Services.AppAuth.Models;

namespace VKVideoReviews.BL.Clients.Interfaces;

public interface IVkApiMethodsClient
{
    Task<VkApiUserResponse> GetUserAsync(string requestParams);
}