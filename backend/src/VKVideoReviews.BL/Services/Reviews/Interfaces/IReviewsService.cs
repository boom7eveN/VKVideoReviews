using VKVideoReviews.BL.Services.Reviews.Models;

namespace VKVideoReviews.BL.Services.Reviews.Interfaces;

public interface IReviewsService
{
    Task<ReviewModel> CreateReviewAsync(Guid userId, Guid videoId, CreateReviewModel createReviewModel);
    Task DeleteReviewAsync(Guid userId, Guid videoId);
    Task<ReviewModel> GetReviewAsync(Guid reviewId);
    Task<IEnumerable<ReviewModel>> GetAllReviewAsync();
    Task<ReviewModel> UpdateReviewAsync(Guid userId, Guid videoId, UpdateReviewModel updateReviewModel);
}