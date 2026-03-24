using System.Net.Http.Json;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Exceptions.VkApiMethodsExceptions;
using VKVideoReviews.BL.Services.AppAuth.Models;

namespace VKVideoReviews.BL.Clients;

public class VkApiMethodsClient(HttpClient httpClient) : IVkApiMethodsClient
{
    public async Task<VkApiUserResponse> GetUserAsync(string requestParams)
    {
        var url = $"users.get?{requestParams}";
        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new VkApiException("VK API returned an unexpected status code", "VK_API_ERROR");
        }
        else
        {
            var vkUser = await response.Content.ReadFromJsonAsync<VkApiResponse<VkApiUserResponse>>();
            if (vkUser?.Response is null)
            {
                throw new InvalidOperationException("Failed to deserialize VK API response");
            }
            return vkUser.Response[0];
        }
    }
}