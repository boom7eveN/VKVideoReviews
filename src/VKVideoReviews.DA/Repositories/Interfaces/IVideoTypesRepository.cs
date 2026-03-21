using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IVideoTypesRepository : IRepository<VideoTypeEntity>
{
    Task<VideoTypeEntity?> GetByTitleAsync(string title);
}