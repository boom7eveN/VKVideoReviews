using System.Net.Http.Json;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Exceptions.VkAuthExceptions;
using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Clients;

public class VkIdClient(HttpClient httpClient) : IVkIdClient
{
    private readonly HttpClient _httpClient = httpClient;


    public async Task<VkTokensApiResponse> GetUserTokensAsync(FormUrlEncodedContent content)
    {
        var response = await httpClient.PostAsync(
            "/oauth2/auth",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<VkTokensApiErrorResponse>();
            throw new VkAuthException(
                errorResponse?.Error ?? "Unknown VK API error",
                errorResponse?.ErrorDescription ?? "Unknown VK API error",
                (int)response.StatusCode
            );
        }
        else
        {
            var vkTokens = await response.Content.ReadFromJsonAsync<VkTokensApiResponse>();
            return vkTokens!;
        }
    }
}