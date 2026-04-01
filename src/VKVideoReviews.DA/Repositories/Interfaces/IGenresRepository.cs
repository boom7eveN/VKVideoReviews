using System.Linq.Expressions;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IGenresRepository : IRepository<GenreEntity>
{
    Task<GenreEntity?> GetGenreByTitleAsync(string title);
    Task<IEnumerable<GenreEntity>> GetAllGenresAsync(Expression<Func<GenreEntity, bool>> predicate);
}