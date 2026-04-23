using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IFavoriteRepository
{
    Task<FavoriteEntity> CreateFavoriteAsync(FavoriteEntity favorite);
    Task<FavoriteEntity?> GetFavoriteWithVideoAsync(Guid userId, Guid videoId);
    Task<IEnumerable<FavoriteEntity>> GetAllFavoriteByUserIdWithVideoAsync(Guid userId);

    Task<(IReadOnlyList<FavoriteEntity> Items, int TotalCount)> GetFavoritesByUserPagedWithVideoAsync(
        Guid userId,
        int pageNumber,
        int pageSize);

    void DeleteFavorite(FavoriteEntity favorite);
    Task<FavoriteEntity?> GetFavoriteByUserAndVideoIdsAsync(Guid userId, Guid videoId);
}