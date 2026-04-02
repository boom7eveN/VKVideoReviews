using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Exceptions.VkAuthExceptions;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Requests;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;
using VKVideoReviews.BL.Services.VkAuth.Interfaces;
using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Services.VkAuth;

public class VkAuthService(string clientId, string redirectUri, IVkApiAuthClient vkApiAuthClient, IMemoryCache cache)
    : IVkAuthService
{
    private const string PkcePrefix = "vk_pkce_";
    private const string StatePrefix = "vk_state_";
    private static readonly TimeSpan StateLifeTime = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan PkceDataLifeTime = TimeSpan.FromMinutes(10);

    public string BuildAuthorizationUrl()
    {
        var pkceData = GeneratePkce();
        var state = GenerateState();
        cache.Set($"{StatePrefix}{state}", state, StateLifeTime);
        cache.Set($"{PkcePrefix}{state}", pkceData.CodeVerifier, PkceDataLifeTime);
        return GetVkAuthorizationUrl(pkceData, state);
    }

    public async Task<VkTokensApiResponse> ExchangeCodeForTokenAsync(VkAuthCallbackModel vkAuthCallbackModel)
    {
        var stateCacheKey = $"{StatePrefix}{vkAuthCallbackModel.State}";
        if (!cache.TryGetValue(stateCacheKey, out string? savedState) || savedState == null)
            throw new StateValidationException();

        cache.Remove(stateCacheKey);

        var pkceCacheKey = $"{PkcePrefix}{vkAuthCallbackModel.State}";
        if (!cache.TryGetValue(pkceCacheKey, out string? codeVerifier) || string.IsNullOrEmpty(codeVerifier))
            throw new PkceValidationException();

        cache.Remove(pkceCacheKey);

        var tokens = await ExchangeCodeForTokenAsync(vkAuthCallbackModel, codeVerifier);
        return tokens;
    }

    private async Task<VkTokensApiResponse> ExchangeCodeForTokenAsync(VkAuthCallbackModel vkAuthCallbackModel,
        string codeVerifier)
    {
        var state = GenerateState();
        cache.Set($"{StatePrefix}{state}", state, StateLifeTime);

        var vkTokens = await vkApiAuthClient.ExchangeCodeAsync(new VkTokenExchangeRequest
        {
            Code = vkAuthCallbackModel.Code,
            DeviceId = vkAuthCallbackModel.DeviceId,
            State = state,
            CodeVerifier = codeVerifier,
            RedirectUri = redirectUri,
            ClientId = clientId
        });
        var stateCacheKey = $"{StatePrefix}{vkTokens.State}";
        if (!cache.TryGetValue(stateCacheKey, out string? savedState) || savedState == null)
            throw new StateValidationException();

        return vkTokens;
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
            CodeChallenge = codeChallenge
        };
    }

    private string GetVkAuthorizationUrl(PkceData data, string state)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "response_type", "code" },
            { "client_id", clientId },
            { "redirect_uri", redirectUri },
            { "state", state },
            { "code_challenge", data.CodeChallenge },
            { "code_challenge_method", "S256" }
        };
        var queryString = string.Join("&", queryParams
            .Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        return $"https://id.vk.ru/authorize?{queryString}";
    }


    private static string GenerateState()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }
}