using System.Security.Cryptography;
using System.Text;
using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.BL.Services.VkAuth;

public class VkAuthService(string clientId, string redirectUri) : IVkAuthService
{
    public PckeData GeneratePkce()
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
        return new PckeData
        {
            CodeVerifier = codeVerifier,
            CodeChallenge = codeChallenge,
        };
    }

    public string BuildAuthorizationUrl(PckeData data, string state)
    {
        var queryParams = new Dictionary<string, string>()
        {
            {"response_type", "code"},
            { "client_id", clientId },
            { "redirect_uri", redirectUri },
            { "state", state },
            { "code_challenge", data.CodeVerifier },
            { "code_challenge_method", "S256" },
        };
        var queryString = string.Join("&", queryParams
            .Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        return $"https://id.vk.ru/authorize?{queryString}";
        
    }
}