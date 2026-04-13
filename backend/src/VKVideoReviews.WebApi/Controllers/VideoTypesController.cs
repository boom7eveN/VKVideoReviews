using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.VideoTypes.Interfaces;
using VKVideoReviews.BL.Services.VideoTypes.Models;
using VKVideoReviews.WebApi.Controllers.Requests.VideoType;
using VKVideoReviews.WebApi.Controllers.Responses.VideoType;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoTypesController(
    IVideoTypesService videoTypesService,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [Route("")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VideoTypesListResponse>> CreateVideoType(
        [FromBody] CreateVideoTypeRequest createVideoTypeRequest)
    {
        var createVideoTypeModel = mapper.Map<CreateVideoTypeModel>(createVideoTypeRequest);
        var videoTypeModel = await videoTypesService.CreateVideoTypeAsync(createVideoTypeModel);
        return Ok(new VideoTypesListResponse([mapper.Map<VideoTypeResponse>(videoTypeModel)]));
    }

    [HttpGet("")]
    [AllowAnonymous]
    public async Task<ActionResult<VideoTypesListResponse>> GetAllVideoTypes()
    {
        var videoTypeModels = await videoTypesService.GetAllVideoTypesAsync();
        return Ok(new VideoTypesListResponse(mapper.Map<List<VideoTypeResponse>>(videoTypeModels)));
    }

    [HttpGet("{videoTypeId}")]
    [AllowAnonymous]
    public async Task<ActionResult<VideoTypesListResponse>> GetVideoTypeById(Guid videoTypeId)
    {
        var videoTypeModel = await videoTypesService.GetVideoTypeByIdAsync(videoTypeId);
        return Ok(new VideoTypesListResponse([mapper.Map<VideoTypeResponse>(videoTypeModel)]));
    }

    [HttpPut("{videoTypeId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VideoTypesListResponse>> UpdateVideoType(Guid videoTypeId,
        [FromBody] UpdateVideoTypeRequest updateVideoTypeRequest)
    {
        var updateVideoTypeModel = mapper.Map<UpdateVideoTypeModel>(updateVideoTypeRequest);
        var videoTypeModel = await videoTypesService.UpdateVideoTypeAsync(videoTypeId, updateVideoTypeModel);
        return Ok(new VideoTypesListResponse([mapper.Map<VideoTypeResponse>(videoTypeModel)]));
    }

    [HttpDelete("{videoTypeId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteVideoType(Guid videoTypeId)
    {
        await videoTypesService.DeleteVideoTypeAsync(videoTypeId);
        return NoContent();
    }
}