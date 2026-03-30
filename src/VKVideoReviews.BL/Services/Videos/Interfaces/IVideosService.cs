using VKVideoReviews.BL.Services.Videos.Models;

namespace VKVideoReviews.BL.Services.Videos.Interfaces;

public interface IVideosService
{
    Task<VideoModel> CreateVideoAsync(CreateVideoModel model);
    Task<IEnumerable<VideoModel>> GetAllVideosAsync();
    Task<VideoModel> GetVideoByIdAsync(Guid id);
    Task<VideoModel> UpdateVideoTypeAsync(Guid id, UpdateVideoModel model);
    Task DeleteVideoTypeAsync(Guid id);
}