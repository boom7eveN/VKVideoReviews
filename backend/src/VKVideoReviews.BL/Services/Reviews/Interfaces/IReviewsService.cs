using VKVideoReviews.BL.Common.Pagination;
using VKVideoReviews.BL.Services.Reviews.Models;

namespace VKVideoReviews.BL.Services.Reviews.Interfaces;

public interface IReviewsService
{
    Task<ReviewModel> CreateReviewAsync(Guid userId, Guid videoId, CreateReviewModel createReviewModel);
    Task DeleteReviewAsync(Guid userId, Guid videoId);
    Task<ReviewModel> GetReviewAsync(Guid reviewId);
    Task<PagedListModel<ReviewModel>> GetAllReviewsPagedAsync(PageRequestModel pageRequest);
    Task<PagedListModel<ReviewModel>> GetReviewsByVideoPagedAsync(Guid videoId, PageRequestModel pageRequest);
    Task<PagedListModel<ReviewModel>> GetMyReviewsPagedAsync(Guid userId, PageRequestModel pageRequest);
    Task<ReviewModel> UpdateReviewAsync(Guid userId, Guid videoId, UpdateReviewModel updateReviewModel);
}