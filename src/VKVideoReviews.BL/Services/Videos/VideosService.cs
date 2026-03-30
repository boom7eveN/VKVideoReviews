using VKVideoReviews.BL.Services.Videos.Interfaces;
using VKVideoReviews.BL.Services.Videos.Models;

namespace VKVideoReviews.BL.Services.Videos;

public class VideosService : IVideosService
{
    public Task<VideoModel> CreateVideoAsync(CreateVideoModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<VideoModel>> GetAllVideosAsync()
    {
        throw new NotImplementedException();
    }

    public Task<VideoModel> GetVideoByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<VideoModel> UpdateVideoTypeAsync(Guid id, UpdateVideoModel model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteVideoTypeAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}