using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VKVideoReviews.BL.Services.Genres.Interfaces;
using VKVideoReviews.BL.Services.Genres.Models;
using VKVideoReviews.WebApi.Controllers.Requests.Genre;
using VKVideoReviews.WebApi.Controllers.Requests.Genres;
using VKVideoReviews.WebApi.Controllers.Responses.Genres;

namespace VKVideoReviews.WebApi.Controllers;

[ApiController]
[Route("api/genres")]
public class GenresController(IGenresService genresService, IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GenresListResponse>> CreateGenre(
        [FromBody] CreateGenreRequest createGenreRequest)
    {
        var createGenreModel = mapper.Map<CreateGenreModel>(createGenreRequest);
        var genreModel = await genresService.CreateGenreAsync(createGenreModel);
        return Ok(new GenresListResponse([mapper.Map<GenreResponse>(genreModel)]));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<GenresListResponse>> GetAllGenres()
    {
        var genreModels = await genresService.GetAllGenresAsync();
        return Ok(new GenresListResponse(mapper.Map<List<GenreResponse>>(genreModels)));
    }

    [HttpGet("{genreId}")]
    [AllowAnonymous]
    public async Task<ActionResult<GenresListResponse>> GetGenreById(Guid genreId)
    {
        var genreModel = await genresService.GetGenreByIdAsync(genreId);
        return Ok(new GenresListResponse([mapper.Map<GenreResponse>(genreModel)]));
    }

    [HttpPut("{genreId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GenresListResponse>> UpdateGenre(Guid genreId,
        [FromBody] UpdateGenreRequest updateGenreRequest)
    {
        var updateGenreModel = mapper.Map<UpdateGenreModel>(updateGenreRequest);
        var genreModel = await genresService.UpdateGenreAsync(genreId, updateGenreModel);
        return Ok(new GenresListResponse([mapper.Map<GenreResponse>(genreModel)]));
    }

    [HttpDelete("{genreId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGenre(Guid genreId)
    {
        await genresService.DeleteGenreAsync(genreId);
        return NoContent();
    }
}