using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IReviewsRepository
{
    Task<ReviewEntity?> CreateAsync(ReviewEntity review);
    Task<ReviewEntity?> GetByReviewIdWithUserAndVideoAsync(Guid reviewId);
    Task<ReviewEntity?> GetByUserAndVideoIdsWithUserAndVideoAsync(Guid userId, Guid videoId);
    void Delete(ReviewEntity review);
    Task<IEnumerable<ReviewEntity>> GetAllWithUserAndVideoAsync();
    void Update(ReviewEntity review);
}