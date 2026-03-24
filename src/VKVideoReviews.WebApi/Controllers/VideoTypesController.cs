using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.VideoTypes.Interfaces;
using VKVideoReviews.BL.Services.VideoTypes.Models;
using VKVideoReviews.WebApi.Controllers.Requests.VideoType;
using VKVideoReviews.WebApi.Controllers.Responses.VideoType;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VideoTypesController(
    ILogger<VideoTypesController> logger,
    IVideoTypesService videoTypesService,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VideoTypesListResponse>> CreateVideoType([FromBody] CreateVideoTypeRequest request)
    {
        var createVideoTypeModel = mapper.Map<CreateVideoTypeModel>(request);
        VideoTypeModel videoTypeModel = await videoTypesService.CreateVideoTypeAsync(createVideoTypeModel);
        return Ok(new VideoTypesListResponse([videoTypeModel]));
    }

    [HttpGet("")]
    [AllowAnonymous]
    public async Task<ActionResult<VideoTypesListResponse>> GetAllVideoTypes()
    {
        var videoTypes = await videoTypesService.GetAllVideoTypesAsync();
        return Ok(new VideoTypesListResponse(videoTypes as List<VideoTypeModel>));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<VideoTypesListResponse>> GetVideoTypeById(Guid id)
    {
        var videoType = await videoTypesService.GetVideoTypeByIdAsync(id);
        return Ok(new VideoTypesListResponse([videoType]));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VideoTypesListResponse>> UpdateVideoType(Guid id,
        [FromBody] UpdateVideoTypeRequest request)
    {
        var updateVideoTypeModel = mapper.Map<UpdateVideoTypeModel>(request);
        var updatedVideoType = await videoTypesService.UpdateVideoTypeAsync(id, updateVideoTypeModel);
        return Ok(new VideoTypesListResponse([updatedVideoType]));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteVideoType(Guid id)
    {
        await videoTypesService.DeleteVideoTypeAsync(id);
        return NoContent();
    }
}