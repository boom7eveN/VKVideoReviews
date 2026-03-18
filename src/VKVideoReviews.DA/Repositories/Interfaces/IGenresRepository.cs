using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IGenresRepository : IRepository<GenreEntity>
{
    Task<GenreEntity?> GetByTitleAsync(string title);
}