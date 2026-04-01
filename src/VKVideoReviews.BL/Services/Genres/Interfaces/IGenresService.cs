using VKVideoReviews.BL.Services.Genres.Models;

namespace VKVideoReviews.BL.Services.Genres.Interfaces;

public interface IGenresService
{
    Task<GenreModel> CreateGenreAsync(CreateGenreModel createGenreModel);
    Task<IEnumerable<GenreModel>> GetAllGenresAsync();
    Task<GenreModel> GetGenreByIdAsync(Guid genreId);
    Task<GenreModel> UpdateGenreAsync(Guid genreId, UpdateGenreModel updateGenreModel);
    Task DeleteGenreAsync(Guid genreId);
}