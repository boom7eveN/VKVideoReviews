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
    public async Task<ActionResult<VideosListResponse>> CreateVideo([FromBody] CreateVideoRequest request)
    {
        var createVideoModel = mapper.Map<CreateVideoModel>(request);
        var videoModel = await videosService.CreateVideoAsync(createVideoModel);
        return Ok(new VideosListResponse([mapper.Map<VideoResponse>(videoModel)]));
    }

    [HttpGet("")]
    [AllowAnonymous]
    public async Task<ActionResult<VideosListResponse>> GetAllVideos()
    {
        var videos = await videosService.GetAllVideosAsync();
        return Ok(new VideosListResponse(mapper.Map<List<VideoResponse>>(videos)));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<VideosListResponse>> GetAllVideos(Guid id)
    {
        var video = await videosService.GetVideoByIdAsync(id);
        return Ok(new VideosListResponse([mapper.Map<VideoResponse>(video)]));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteVideo(Guid id)
    {
        await videosService.DeleteVideoAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VideosListResponse>> UpdateVideo(Guid id, [FromBody] UpdateVideoRequest request)
    {
        var updateVideoModel = mapper.Map<UpdateVideoModel>(request);
        var updatedVideo = await videosService.UpdateVideoAsync(id, updateVideoModel);
        return Ok(new VideosListResponse([mapper.Map<VideoResponse>(updatedVideo)]));
    }
}