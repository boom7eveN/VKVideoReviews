using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using VKVideoReviews.BL.Exceptions.VkAuthExceptions;
using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Services.VkAuth;

public class VkAuthService(string clientId, string redirectUri, HttpClient httpClient, IMemoryCache cache)
    : IVkAuthService
{
    private const string PkcePrefix = "vk_pkce_";
    private const string StatePrefix = "vk_state_";
    private static readonly TimeSpan StateLifeTime = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan PkceDataLifeTime = TimeSpan.FromMinutes(10);

    public string BuildAuthorizationUrl()
    {
        PkceData pkceData = GeneratePkce();
        string state = GenerateState();
        cache.Set($"{StatePrefix}{state}", state, StateLifeTime);
        cache.Set($"{PkcePrefix}{state}", pkceData.CodeVerifier, PkceDataLifeTime);
        return GetVkAuthorizationUrl(pkceData, state);
    }

    public async Task<string> ProcessCallback(VkAuthCallbackModel vkAuthCallbackModel)
    {
        var stateCacheKey = $"{StatePrefix}{vkAuthCallbackModel.State}";
        if (!cache.TryGetValue(stateCacheKey, out string? savedState) || savedState == null)
        {
            throw new StateValidationException();
        }

        cache.Remove(stateCacheKey);

        var pkceCacheKey = $"{PkcePrefix}{vkAuthCallbackModel.State}";
        if (!cache.TryGetValue(pkceCacheKey, out string? codeVerifier) || string.IsNullOrEmpty(codeVerifier))
        {
            throw new PkceValidationException();
        }

        cache.Remove(pkceCacheKey);

        VkTokensApiResponse tokens = await ExchangeCodeForTokenAsync(vkAuthCallbackModel, codeVerifier);
        return tokens.AccessToken;
    }

    private async Task<VkTokensApiResponse> ExchangeCodeForTokenAsync(VkAuthCallbackModel vkAuthCallbackModel,
        string codeVerifier)
    {
        var state = GenerateState();
        cache.Set($"{StatePrefix}{state}", state, StateLifeTime);

        var content = GetExchangeCodeForTokenParameters(vkAuthCallbackModel, codeVerifier, state);
        var response = await httpClient.PostAsync(
            "https://id.vk.ru/oauth2/auth",
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
            var stateCacheKey = $"{StatePrefix}{vkTokens?.State}";
            if (!cache.TryGetValue(stateCacheKey, out string? savedState) || savedState == null)
            {
                throw new StateValidationException();
            }

            return vkTokens!;
        }
    }


    private static PkceData GeneratePkce()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(32);
        var codeVerifier = Convert.ToBase64String(randomBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');

        var hash = SHA256.HashData(Encoding.ASCII.GetBytes(codeVerifier));
        var codeChallenge = Convert.ToBase64String(hash)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');

        return new PkceData
        {
            CodeVerifier = codeVerifier,
            CodeChallenge = codeChallenge,
        };
    }

    private string GetVkAuthorizationUrl(PkceData data, string state)
    {
        var queryParams = new Dictionary<string, string>()
        {
            { "response_type", "code" },
            { "client_id", clientId },
            { "redirect_uri", redirectUri },
            { "state", state },
            { "code_challenge", data.CodeChallenge },
            { "code_challenge_method", "S256" },
        };
        var queryString = string.Join("&", queryParams
            .Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        return $"https://id.vk.ru/authorize?{queryString}";
    }

    private FormUrlEncodedContent GetExchangeCodeForTokenParameters(VkAuthCallbackModel vkAuthCallbackModel,
        string codeVerifier, string state)
    {
        var requestParams = new Dictionary<string, string>()
        {
            { "grant_type", "authorization_code" },
            { "code_verifier", codeVerifier },
            { "redirect_uri", redirectUri },
            { "code", vkAuthCallbackModel.Code },
            { "client_id", clientId },
            { "device_id", vkAuthCallbackModel.DeviceId },
            { "state", state },
        };
        var content = new FormUrlEncodedContent(requestParams);
        return content;
    }


    private static string GenerateState()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }
}