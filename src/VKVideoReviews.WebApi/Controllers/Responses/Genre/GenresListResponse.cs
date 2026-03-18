using VKVideoReviews.BL.Models;

namespace VKVideoReviews.WebApi.Controllers.Responses.Genre;

public record GenresListResponse(List<GenreModel> Genres);