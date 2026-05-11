using VKVideoReviews.BL.Common.Pagination;
using VKVideoReviews.BL.Services.Favorite.Models;

namespace VKVideoReviews.BL.Services.Favorite.Interfaces;

public interface IFavoriteService
{
    Task<FavoriteModel> CreateFavoriteAsync(Guid userId, CreateFavoriteModel createFavoriteModel);
    Task<PagedListModel<FavoriteModel>> GetMyFavoritePagedAsync(Guid userId, PageRequestModel pageRequest);
    Task<FavoriteModel> GetFavoriteAsync(Guid userId, Guid videoId);
    Task DeleteFavoriteAsync(Guid userId, Guid videoId);
}