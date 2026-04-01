using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.Videos.Interfaces;
using VKVideoReviews.BL.Services.Videos.Models;
using VKVideoReviews.WebApi.Controllers.Requests.Videos;
using VKVideoReviews.WebApi.Controllers.Responses.Videos;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideosController(IVideosService videosService, IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [Route("")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VideosListResponse>> CreateVideo([FromBody] CreateVideoRequest createVideoRequest)
    {
        var createVideoModel = mapper.Map<CreateVideoModel>(createVideoRequest);
        var videoModel = await videosService.CreateVideoAsync(createVideoModel);
        return Ok(new VideosListResponse([mapper.Map<VideoResponse>(videoModel)]));
    }

    [HttpGet("")]
    [AllowAnonymous]
    public async Task<ActionResult<VideosListResponse>> GetAllVideos()
    {
        var videoModels = await videosService.GetAllVideosAsync();
        return Ok(new VideosListResponse(mapper.Map<List<VideoResponse>>(videoModels)));
    }

    [HttpGet("{videoId}")]
    [AllowAnonymous]
    public async Task<ActionResult<VideosListResponse>> GetAllVideos(Guid videoId)
    {
        var videoModel = await videosService.GetVideoByIdAsync(videoId);
        return Ok(new VideosListResponse([mapper.Map<VideoResponse>(videoModel)]));
    }

    [HttpDelete("{videoId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteVideo(Guid videoId)
    {
        await videosService.DeleteVideoAsync(videoId);
        return NoContent();
    }

    [HttpPut("{videoId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VideosListResponse>> UpdateVideo(
        Guid videoId,
        [FromBody] UpdateVideoRequest updateVideoRequest)
    {
        var updateVideoModel = mapper.Map<UpdateVideoModel>(updateVideoRequest);
        var videoModel = await videosService.UpdateVideoAsync(videoId, updateVideoModel);
        return Ok(new VideosListResponse([mapper.Map<VideoResponse>(videoModel)]));
    }
}