using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.Genres.Interfaces;
using VKVideoReviews.BL.Services.Genres.Models;
using VKVideoReviews.WebApi.Controllers.Requests.Genre;
using VKVideoReviews.WebApi.Controllers.Responses.Genre;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GenresController(ILogger<GenresController> logger, IGenresService genresService, IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GenresListResponse>> CreateGenre([FromBody] CreateGenreRequest request)
    {
        var createGenreModel = mapper.Map<CreateGenreModel>(request);
        GenreModel genreModel = await genresService.CreateGenreAsync(createGenreModel);
        return Ok(new GenresListResponse([genreModel]));
    }

    [HttpGet("")]
    [AllowAnonymous]
    public async Task<ActionResult<GenresListResponse>> GetAllGenres()
    {
        var genres = await genresService.GetAllGenresAsync();
        return Ok(new GenresListResponse(genres as List<GenreModel>));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<GenresListResponse>> GetGenreById(Guid id)
    {
        var genre = await genresService.GetGenreByIdAsync(id);
        return Ok(new GenresListResponse([genre]));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GenresListResponse>> UpdateGenre(Guid id, [FromBody] UpdateGenreRequest request)
    {
        var updateGenreModel = mapper.Map<UpdateGenreModel>(request);
        var updatedGenre = await genresService.UpdateGenreAsync(id, updateGenreModel);
        return Ok(new GenresListResponse([updatedGenre]));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        await genresService.DeleteGenreAsync(id);
        return NoContent();
    }
}