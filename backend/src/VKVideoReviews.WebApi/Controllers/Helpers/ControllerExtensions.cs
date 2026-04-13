using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace VKVideoReviews.WebApi.Controllers.Helpers;

public static class ControllerExtensions
{
    public static Guid GetCurrentUserId(this ControllerBase controller)
    {
        var userIdClaim = controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("User is not authorized");

        return userId;
    }
}