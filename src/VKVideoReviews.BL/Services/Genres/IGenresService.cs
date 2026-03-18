using VKVideoReviews.BL.Services.Genres.Models;

namespace VKVideoReviews.BL.Services.Genres;

public interface IGenresService
{
    Task<GenreModel> CreateGenreAsync(CreateGenreModel model);
    Task<IEnumerable<GenreModel>> GetAllGenresAsync();
    Task<GenreModel> GetGenreByIdAsync(Guid id);
    Task<GenreModel> UpdateGenreAsync(Guid id, UpdateGenreModel model);
    Task DeleteGenreAsync(Guid id);
}