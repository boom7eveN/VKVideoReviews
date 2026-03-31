using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IGenresVideosRepository
{
    Task AddRangeAsync(IEnumerable<GenresVideosEntity> entities);
    Task DeleteByVideoIdAsync(Guid videoId);
}