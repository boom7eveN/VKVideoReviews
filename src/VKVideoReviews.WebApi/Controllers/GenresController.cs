using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.WebApi.Controllers.Requests.Genre;
using VKVideoReviews.WebApi.Controllers.Responses.Genre;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GenresController : ControllerBase
{
    private readonly ILogger<GenresController> _logger;

    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<GenresListResponse>> CreateGenre([FromBody] CreateGenreRequest request)
    {
        return Ok(new GenresListResponse([]));
    }
}