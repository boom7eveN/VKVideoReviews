using System.Linq.Expressions;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IGenresRepository : IRepository<GenreEntity>
{
    Task<GenreEntity?> GetByTitleAsync(string title);
    Task<IEnumerable<GenreEntity>> GetAllAsync(Expression<Func<GenreEntity, bool>> predicate);
}