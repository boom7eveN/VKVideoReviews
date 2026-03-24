using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.BL.Services.VkAuth;
using VKVideoReviews.BL.Services.VkAuth.Interfaces;
using VKVideoReviews.BL.Services.VkAuth.Models;
using VKVideoReviews.WebApi.Controllers.Requests.VkAuth;
using VKVideoReviews.WebApi.Controllers.Responses.AppAuth;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VkAuthController(
    IAppAuthService appAuthService,
    IVkAuthService vkAuthService,
    IMapper mapper) : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        return Redirect(vkAuthService.BuildAuthorizationUrl());
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] VkAuthCallbackRequest request)
    {
        var vkAuthCallbackModel = mapper.Map<VkAuthCallbackModel>(request);
        var vkTokens = await vkAuthService.ExchangeCodeForTokenAsync(vkAuthCallbackModel);
        var authTokensResult = await appAuthService.SignInWithVkTokensAsync(vkTokens);
        return Ok(mapper.Map<AuthTokensResponse>(authTokensResult));
    }
}