using VKVideoReviews.BL.Services.Genres.Models;

namespace VKVideoReviews.WebApi.Controllers.Responses.Genre;

public record GenresListResponse(List<GenreResponse> Genres);