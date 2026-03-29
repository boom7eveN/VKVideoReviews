using System.Net.Http.Json;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Exceptions.VkAuthExceptions;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Requests;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;

namespace VKVideoReviews.BL.Clients;

public class VkApiAuthClient(HttpClient httpClient) : IVkApiAuthClient
{
    public async Task<VkTokensApiResponse> ExchangeCodeAsync(VkTokenExchangeRequest request)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code_verifier", request.CodeVerifier },
            { "redirect_uri", request.RedirectUri },
            { "code", request.Code },
            { "client_id", request.ClientId },
            { "device_id", request.DeviceId },
            { "state", request.State },
        });

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
            if (vkTokens is null)
            {
                throw new InvalidOperationException("Failed to deserialize VK tokens response");
            }

            return vkTokens;
        }
    }
}