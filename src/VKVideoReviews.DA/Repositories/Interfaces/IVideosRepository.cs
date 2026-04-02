using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IVideosRepository
{
    Task<VideoEntity?> CreateVideoAsync(VideoEntity videoEntity);
    Task<VideoEntity?> GetVideoByIdAsync(Guid videoId);
    void DeleteVideo(VideoEntity videoEntity);
    void UpdateVideo(VideoEntity videoEntity);
    Task<VideoEntity?> GetVideoByIdWithGenresAndVideotypeAsync(Guid videoId);
    Task<IEnumerable<VideoEntity>> GetAllVideosWithGenresAndVideotypeAsync();
    Task UpdateVideoRatingByIdAsync(Guid videoId);
    Task<VideoEntity?> LockForUpdateByIdAsync(Guid videoId);
}