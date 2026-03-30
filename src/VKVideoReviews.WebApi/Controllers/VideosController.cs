using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.Videos.Interfaces;
using VKVideoReviews.BL.Services.Videos.Models;
using VKVideoReviews.WebApi.Controllers.Requests.Videos;
using VKVideoReviews.WebApi.Controllers.Responses.Videos;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VideosController(ILogger<GenresController> logger, IVideosService videosService, IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [Route("")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VideosListResponse>> CreateGenre([FromBody] CreateVideoRequest request)
    {
        var createVideoModel = mapper.Map<CreateVideoModel>(request);
        var videoModel = await videosService.CreateVideoAsync(createVideoModel);
        return Ok(new VideosListResponse([mapper.Map<VideoResponse>(videoModel)]));
    }
}