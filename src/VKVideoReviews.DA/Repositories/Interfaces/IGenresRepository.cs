using System.Linq.Expressions;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IGenresRepository
{
    Task<GenreEntity?> CreateGenreAsync(GenreEntity genreEntity);
    Task<IEnumerable<GenreEntity>> GetAllGenresAsync();
    Task<GenreEntity?> GetGenreByIdAsync(Guid genreId);
    void DeleteGenre(GenreEntity genreEntity);
    void UpdateGenre(GenreEntity genreEntity);
    Task<GenreEntity?> GetGenreByTitleAsync(string title);
    Task<IEnumerable<GenreEntity>> GetAllGenresAsync(Expression<Func<GenreEntity, bool>> predicate);
}