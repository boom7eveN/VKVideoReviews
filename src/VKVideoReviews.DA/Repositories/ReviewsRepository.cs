using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class ReviewsRepository(VkVideoReviewsDbContext context) : IReviewsRepository
{
    public async Task<ReviewEntity?> CreateReviewAsync(ReviewEntity review)
    {
        var maybeReview = await context.Reviews.FirstOrDefaultAsync(r =>
            r.UserId == review.UserId &&
            r.VideoId == review.VideoId);

        if (maybeReview is not null)
            return null;

        await context.Reviews.AddAsync(review);
        return review;
    }

    public async Task<ReviewEntity?> GetReviewByIdWithUserAndVideoAsync(Guid reviewId)
    {
        return await context.Reviews
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Video)
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
    }

    public async Task<ReviewEntity?> GetReviewByUserAndVideoIdsWithUserAndVideoAsync(Guid userId, Guid videoId)
    {
        return await context.Reviews
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Video)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.VideoId == videoId);
    }

    public void DeleteReview(ReviewEntity review)
    {
        context.Reviews.Remove(review);
    }

    public async Task<IEnumerable<ReviewEntity>> GetAllReviewsWithUsersAndVideosAsync()
    {
        return await context.Reviews
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Video)
            .ToListAsync();
    }

    public void UpdateReview(ReviewEntity review)
    {
        context.Reviews.Update(review);
    }

    public async Task<ReviewEntity?> GetReviewByUserAndVideoIdsAsync(Guid userId, Guid videoId)
    {
        return await context.Reviews
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.UserId == userId && r.VideoId == videoId);
    }
}