using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.BL.Services.VkAuth.Interfaces;
using VKVideoReviews.BL.Services.VkAuth.Models;
using VKVideoReviews.WebApi.Controllers.Requests.VkAuth;
using VKVideoReviews.WebApi.Settings;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("api/auth/vk")]
public class VkAuthController(
    IAppAuthService appAuthService,
    IVkAuthService vkAuthService,
    IMapper mapper,
    AppSettings appSettings) : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        return Redirect(vkAuthService.BuildAuthorizationUrl());
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] VkAuthCallbackRequest vkAuthCallbackRequest)
    {
        var frontendBaseUrl = appSettings.FrontendBaseUrl.TrimEnd('/');

        try
        {
            var vkAuthCallbackModel = mapper.Map<VkAuthCallbackModel>(vkAuthCallbackRequest);
            var vkTokens = await vkAuthService.ExchangeCodeForTokenAsync(vkAuthCallbackModel);
            var authTokens = await appAuthService.SignInWithVkTokensAsync(vkTokens);

            var query =
                $"status=ok" +
                $"&accessToken={Uri.EscapeDataString(authTokens.AccessToken)}" +
                $"&refreshToken={Uri.EscapeDataString(authTokens.RefreshToken)}" +
                $"&expiresIn={authTokens.ExpiresInSeconds}";

            return Redirect($"{frontendBaseUrl}/auth/callback?{query}");
        }
        catch
        {
            return Redirect($"{frontendBaseUrl}/auth/callback?status=error");
        }
    }
}
