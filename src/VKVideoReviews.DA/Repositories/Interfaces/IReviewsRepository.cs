using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IReviewsRepository
{
    Task<ReviewEntity?> CreateReviewAsync(ReviewEntity review);
    Task<ReviewEntity?> GetReviewByIdWithUserAndVideoAsync(Guid reviewId);
    Task<ReviewEntity?> GetReviewByUserAndVideoIdsWithUserAndVideoAsync(Guid userId, Guid videoId);
    void DeleteReview(ReviewEntity review);
    Task<IEnumerable<ReviewEntity>> GetAllReviewsWithUsersAndVideosAsync();
    void UpdateReview(ReviewEntity review);
    Task<ReviewEntity?> GetReviewByUserAndVideoIdsAsync(Guid userId, Guid videoId);
}