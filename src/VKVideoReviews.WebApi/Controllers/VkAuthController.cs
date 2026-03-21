using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using VKVideoReviews.BL.Services.VkAuth;
using VKVideoReviews.BL.Services.VkAuth.Models;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VkAuthController(
    IVkAuthService service,
    IMemoryCache cache,
    ILogger<VkAuthController> logger) : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        PckeData pkceData = service.GeneratePkce();

        string state = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');

        cache.Set($"state_{state}", pkceData.CodeVerifier, TimeSpan.FromMinutes(10));

        var authUrl = service.BuildAuthorizationUrl(pkceData, state);

        return Redirect(authUrl);
    }

    [HttpGet("callback")]
    public IActionResult Callback(
        [FromQuery] string code,
        [FromQuery] string state,
        [FromQuery] string device_id,
        [FromQuery] string? error,
        [FromQuery] string? error_description)
    {
        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(new { error, error_description });
        }

        var cacheKey = $"state_{state}";
        if (!cache.TryGetValue(cacheKey, out string? codeVerifier) || codeVerifier == null)
        {
            return BadRequest(new { error = "invalid_state", error_description = "State not found" });
        }
        
        cache.Remove(cacheKey);

        return Ok($"{code}:{state}:{device_id} - ЗАГЛУУУУУУУУУУУУУУУУУУУУУУШКА");
    }
}