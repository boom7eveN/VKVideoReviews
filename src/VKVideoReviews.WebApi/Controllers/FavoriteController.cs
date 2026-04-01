using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.Favorite.Interfaces;
using VKVideoReviews.BL.Services.Favorite.Models;
using VKVideoReviews.WebApi.Controllers.Helpers;
using VKVideoReviews.WebApi.Controllers.Requests.Favorite;
using VKVideoReviews.WebApi.Controllers.Responses.Favorite;
using VKVideoReviews.WebApi.Controllers.Responses.Reviews;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoriteController(
    IFavoriteService favoriteService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "User, Admin")] 
    public async Task<ActionResult<FavoriteListResponse>> GetAllFavorite()
    {
        var userId = this.GetCurrentUserId();
        var favoriteModel = await favoriteService.GetAllFavoriteAsync(userId);
        return Ok(new FavoriteListResponse(mapper.Map<List<FavoriteResponse>>(favoriteModel)));
    }
    
    [HttpPost]
    [Authorize(Roles = "User, Admin")] 
    public async Task<ActionResult<FavoriteListResponse>> CreateFavorite([FromBody] CreateFavoriteRequest request)
    {
        var userId = this.GetCurrentUserId();
        var createFavoriteModel = mapper.Map<CreateFavoriteModel>(request);
        var favoriteModel = await favoriteService.CreateFavoriteAsync(userId, createFavoriteModel);
        return Ok(new FavoriteListResponse([mapper.Map<FavoriteResponse>(favoriteModel)]));
    }
    
    [HttpDelete]
    [Authorize(Roles = "User, Admin")] 
    [Route("{videoId}")]
    public async Task<IActionResult> DeleteFavorite(Guid videoId)
    {
        var userId = this.GetCurrentUserId();
        await favoriteService.DeleteFavoriteAsync(userId, videoId);
        return NoContent();
    }
}