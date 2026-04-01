using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IVideosRepository : IRepository<VideoEntity>
{
    Task<VideoEntity?> GetVideoByIdWithGenresAndVideotypeAsync(Guid videoId);
    Task<IEnumerable<VideoEntity>> GetAllVideosWithGenresAndVideotypeAsync();
    Task UpdateVideoRatingByIdAsync(Guid videoId);
    Task<VideoEntity?> LockForUpdateByIdAsync(Guid videoId);
}