using VKVideoReviews.BL.Services.Favorite.Models;

namespace VKVideoReviews.BL.Services.Favorite.Interfaces;

public interface IFavoriteService
{
    Task<FavoriteModel> CreateFavoriteAsync(Guid userId, CreateFavoriteModel model);
    Task<IEnumerable<FavoriteModel>> GetAllFavoriteAsync(Guid userId);
    Task DeleteFavoriteAsync(Guid userId, Guid videoId);
}