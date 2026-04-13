using System.Net.Http.Json;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Exceptions.VkApiMethodsExceptions;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Requests;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;

namespace VKVideoReviews.BL.Clients;

public class VkApiMethodsClient(HttpClient httpClient) : IVkApiMethodsClient
{
    public async Task<VkApiUserResponse> GetUserAsync(VkUserGetRequest request)
    {
        var url =
            $"users.get?user_id={request.UserId}&" +
            $"access_token={Uri.EscapeDataString(request.AccessToken)}&" +
            $"fields={Uri.EscapeDataString(request.Fields)}&" +
            $"v={Uri.EscapeDataString(request.Version)}";
        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            throw new VkApiException("VK API returned an unexpected status code", "VK_API_ERROR");

        var vkUser = await response.Content.ReadFromJsonAsync<VkApiResponse<VkApiUserResponse>>();
        if (vkUser?.Response is null) throw new InvalidOperationException("Failed to deserialize VK API response");

        if (vkUser.Response.Count == 0)
            throw new VkApiException("VK API returned empty response", "VK_API_EMPTY_RESPONSE");

        return vkUser.Response[0];
    }
}