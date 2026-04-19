using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Exceptions.VkAuthExceptions;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Requests;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;
using VKVideoReviews.BL.Services.VkAuth.Interfaces;
using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Services.VkAuth;

public class VkAuthService(
    string clientId,
    string redirectUri,
    IVkApiAuthClient vkApiAuthClient,
    IDistributedCache cache)
    : IVkAuthService
{
    private const string PkcePrefix = "vk_pkce_";
    private const string StatePrefix = "vk_state_";
    private static readonly TimeSpan StateLifeTime = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan PkceDataLifeTime = TimeSpan.FromMinutes(10);

    private static readonly DistributedCacheEntryOptions StateCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = StateLifeTime
    };

    private static readonly DistributedCacheEntryOptions PkceCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = PkceDataLifeTime
    };

    public string BuildAuthorizationUrl()
    {
        var pkceData = GeneratePkce();
        var state = GenerateState();

        var stateKey = $"{StatePrefix}{state}";
        var pkceKey = $"{PkcePrefix}{state}";

        cache.SetString(stateKey, state, StateCacheOptions);
        cache.SetString(pkceKey, pkceData.CodeVerifier, PkceCacheOptions);

        return GetVkAuthorizationUrl(pkceData, state);
    }

    public async Task<VkTokensApiResponse> ExchangeCodeForTokenAsync(VkAuthCallbackModel vkAuthCallbackModel)
    {
        var stateKey = $"{StatePrefix}{vkAuthCallbackModel.State}";
        var savedState = await cache.GetStringAsync(stateKey);

        if (string.IsNullOrEmpty(savedState))
            throw new StateValidationException();

        await cache.RemoveAsync(stateKey);

        var pkceKey = $"{PkcePrefix}{vkAuthCallbackModel.State}";
        var codeVerifier = await cache.GetStringAsync(pkceKey);

        if (string.IsNullOrEmpty(codeVerifier))
            throw new PkceValidationException();

        await cache.RemoveAsync(pkceKey);

        var tokens = await ExchangeCodeForTokenAsync(vkAuthCallbackModel, codeVerifier);
        return tokens;
    }

    private async Task<VkTokensApiResponse> ExchangeCodeForTokenAsync(
        VkAuthCallbackModel vkAuthCallbackModel,
        string codeVerifier)
    {
        var state = GenerateState();
        var stateKey = $"{StatePrefix}{state}";
        await cache.SetStringAsync(stateKey, state, StateCacheOptions);

        var vkTokens = await vkApiAuthClient.ExchangeCodeAsync(new VkTokenExchangeRequest
        {
            Code = vkAuthCallbackModel.Code,
            DeviceId = vkAuthCallbackModel.DeviceId,
            State = state,
            CodeVerifier = codeVerifier,
            RedirectUri = redirectUri,
            ClientId = clientId
        });

        var returnedStateKey = $"{StatePrefix}{vkTokens.State}";
        var savedState = await cache.GetStringAsync(returnedStateKey);

        if (string.IsNullOrEmpty(savedState))
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