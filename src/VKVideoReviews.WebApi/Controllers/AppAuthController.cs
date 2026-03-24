using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.WebApi.Controllers.Requests.AppAuth;
using VKVideoReviews.WebApi.Controllers.Responses.AppAuth;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AppAuthController(IAppAuthService appAuthService, IMapper mapper) : ControllerBase
{
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthTokensResponse>> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await appAuthService.RefreshAsync(request.RefreshToken);
        if (result == null)
            return Unauthorized();
        return Ok(mapper.Map<AuthTokensResponse>(result));
    }
}