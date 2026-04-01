using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IFavoriteRepository
{
    Task<FavoriteEntity?> CreateFavoriteAsync(FavoriteEntity favorite);
    Task<FavoriteEntity?> GetFavoriteWithVideoAsync(Guid userId, Guid videoId);
    Task<IEnumerable<FavoriteEntity>> GetAllFavoriteWithVideoByUserIdAsync(Guid userId);
    void DeleteFavorite(FavoriteEntity favorite);
    Task<FavoriteEntity?> GetFavoriteAsync(Guid userId, Guid videoId);
}