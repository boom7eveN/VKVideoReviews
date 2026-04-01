using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IVideoTypesRepository : IRepository<VideoTypeEntity>
{
    Task<VideoTypeEntity?> GetVideoTypeByTitleAsync(string title);
}