using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Common.Pagination;
using VKVideoReviews.BL.Services.Favorite.Interfaces;
using VKVideoReviews.BL.Services.Favorite.Models;
using VKVideoReviews.WebApi.Controllers.Helpers;
using VKVideoReviews.WebApi.Controllers.Requests.Favorite;
using VKVideoReviews.WebApi.Controllers.Requests.Pagination;
using VKVideoReviews.WebApi.Controllers.Responses.Favorite;
using VKVideoReviews.WebApi.Controllers.Responses.Pagination;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("api/users/me/favorites")]
public class FavoriteController(
    IFavoriteService favoriteService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<PagedResponse<FavoriteResponse>>> GetMyFavorite(
        [FromQuery] PageRequest request)
    {
        var userId = this.GetCurrentUserId();
        var pageRequest = mapper.Map<PageRequestModel>(request);
        var pagedFavorite = await favoriteService.GetMyFavoritePagedAsync(userId, pageRequest);
        return Ok(mapper.Map<PagedResponse<FavoriteResponse>>(pagedFavorite));
    }

    [HttpPost]
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<FavoriteListResponse>> CreateMyFavorite(
        [FromBody] CreateFavoriteRequest createFavoriteRequest)
    {
        var userId = this.GetCurrentUserId();
        var createFavoriteModel = mapper.Map<CreateFavoriteModel>(createFavoriteRequest);
        var favoriteModel = await favoriteService.CreateFavoriteAsync(userId, createFavoriteModel);
        return Ok(new FavoriteListResponse([mapper.Map<FavoriteResponse>(favoriteModel)]));
    }

    [HttpDelete("{videoId:guid}")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> DeleteMyFavorite(Guid videoId)
    {
        var userId = this.GetCurrentUserId();
        await favoriteService.DeleteFavoriteAsync(userId, videoId);
        return NoContent();
    }
}