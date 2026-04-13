using VKVideoReviews.BL.Services.Videos.Models;

namespace VKVideoReviews.BL.Services.Videos.Interfaces;

public interface IVideosService
{
    Task<VideoModel> CreateVideoAsync(CreateVideoModel createVideoModel);
    Task<IEnumerable<VideoModel>> GetAllVideosAsync();
    Task<VideoModel> GetVideoByIdAsync(Guid videoId);
    Task<VideoModel> UpdateVideoAsync(Guid videoId, UpdateVideoModel updateVideoModel);
    Task DeleteVideoAsync(Guid videoId);
}