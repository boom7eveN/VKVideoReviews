using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.BL.Services.VkAuth.Interfaces;
using VKVideoReviews.BL.Services.VkAuth.Models;
using VKVideoReviews.WebApi.Controllers.Requests.VkAuth;
using VKVideoReviews.WebApi.Controllers.Responses.AppAuth;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("api/auth/vk")]
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
    public async Task<IActionResult> Callback([FromQuery] VkAuthCallbackRequest vkAuthCallbackRequest)
    {
        var vkAuthCallbackModel = mapper.Map<VkAuthCallbackModel>(vkAuthCallbackRequest);
        var vkTokens = await vkAuthService.ExchangeCodeForTokenAsync(vkAuthCallbackModel);
        var authTokensModel = await appAuthService.SignInWithVkTokensAsync(vkTokens);
        return Ok(mapper.Map<AuthTokensResponse>(authTokensModel));
    }
}