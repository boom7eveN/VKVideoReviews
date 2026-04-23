using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.WebApi.Controllers.Helpers;
using VKVideoReviews.WebApi.Controllers.Responses.AppAuth;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IAppAuthService appAuthService, IMapper mapper) : ControllerBase
{
    [HttpGet("me")]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<UserResponse>> GetMe()
    {
        var userId = this.GetCurrentUserId();
        var userModel = await appAuthService.GetUserByIdAsync(userId);
        return Ok(mapper.Map<UserResponse>(userModel));
    }
}
