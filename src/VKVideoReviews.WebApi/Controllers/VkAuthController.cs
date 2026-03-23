using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.VkAuth;
using VKVideoReviews.BL.Services.VkAuth.Models;
using VKVideoReviews.WebApi.Controllers.Requests.VkAuth;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VkAuthController(
    IVkAuthService service,
    IMapper mapper) : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        return Redirect(service.BuildAuthorizationUrl());
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] VkAuthCallbackRequest request)
    {
        var vkAuthCallbackModel = mapper.Map<VkAuthCallbackModel>(request);
        var accessToken = await service.ProcessCallback(vkAuthCallbackModel);

        return Ok(accessToken);
    }
}