using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IVideosRepository : IRepository<VideoEntity>
{
    Task<VideoEntity?> GetVideoByIdWithGenresAndVideotypesAsync(Guid id);
    Task<IEnumerable<VideoEntity>> GetAllVideosWithGenresAndVideotypesAsync();
}