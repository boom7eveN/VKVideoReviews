using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IVideoTypesRepository
{
    Task<VideoTypeEntity> CreateVideoTypeAsync(VideoTypeEntity videoTypeEntity);
    Task<IEnumerable<VideoTypeEntity>> GetAllVideoTypesAsync();
    Task<VideoTypeEntity?> GetVideoTypeByIdAsync(Guid videoTypeId);
    void DeleteVideoType(VideoTypeEntity videoTypeEntity);
    void UpdateVideoType(VideoTypeEntity videoTypeEntity);
    Task<VideoTypeEntity?> GetVideoTypeByTitleAsync(string title);
}