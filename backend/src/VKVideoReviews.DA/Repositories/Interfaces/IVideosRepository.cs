using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IVideosRepository
{
    Task<VideoEntity> CreateVideoAsync(VideoEntity videoEntity);
    Task<VideoEntity?> GetVideoByTitleYearAndTypeAsync(string title, int startYear, Guid videoTypeId);
    Task<VideoEntity?> GetVideoByIdAsync(Guid videoId);
    void DeleteVideo(VideoEntity videoEntity);
    void UpdateVideo(VideoEntity videoEntity);
    Task<VideoEntity?> GetVideoByIdWithGenresAndVideotypeAsync(Guid videoId);
    Task<VideoEntity?> GetVideoByIdWithGenresVideotypeAndReviewsAsync(Guid videoId);
    Task<IEnumerable<VideoEntity>> GetAllVideosWithGenresAndVideotypeAsync();

    Task<(IReadOnlyList<VideoEntity> Items, int TotalCount)> GetVideosPagedWithGenresAndVideotypeAsync(
        int pageNumber,
        int pageSize,
        string? titlePart);

    Task UpdateVideoRatingByIdAsync(Guid videoId);
    Task<VideoEntity?> GetVideoByIdLockForUpdateAsync(Guid videoId);
}