using VKVideoReviews.BL.Services.VideoTypes.Models;

namespace VKVideoReviews.BL.Services.VideoTypes;

public interface IVideoTypesService
{
    Task<VideoTypeModel> CreateVideoTypeAsync(CreateVideoTypeModel model);
    Task<IEnumerable<VideoTypeModel>> GetAllVideoTypesAsync();
    Task<VideoTypeModel> GetVideoTypeByIdAsync(Guid id);
    Task<VideoTypeModel> UpdateVideoTypeAsync(Guid id, UpdateVideoTypeModel model);
    Task DeleteVideoTypeAsync(Guid id);
}