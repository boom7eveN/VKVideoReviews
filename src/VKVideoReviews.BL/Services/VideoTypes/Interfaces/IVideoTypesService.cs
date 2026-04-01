using VKVideoReviews.BL.Services.VideoTypes.Models;

namespace VKVideoReviews.BL.Services.VideoTypes.Interfaces;

public interface IVideoTypesService
{
    Task<VideoTypeModel> CreateVideoTypeAsync(CreateVideoTypeModel createVideoTypeModel);
    Task<IEnumerable<VideoTypeModel>> GetAllVideoTypesAsync();
    Task<VideoTypeModel> GetVideoTypeByIdAsync(Guid videoTypeId);
    Task<VideoTypeModel> UpdateVideoTypeAsync(Guid videoTypeId, UpdateVideoTypeModel updateVideoTypeModel);
    Task DeleteVideoTypeAsync(Guid videoTypeId);
}