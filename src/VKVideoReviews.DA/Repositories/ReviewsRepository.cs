using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class ReviewsRepository(VkVideoReviewsDbContext context) : IReviewsRepository
{
    public async Task<ReviewEntity?> CreateAsync(ReviewEntity review)
    {
        var maybeReview = await context.Reviews.FirstOrDefaultAsync(r =>
            r.UserId == review.UserId &&
            r.VideoId == review.VideoId);
        if (maybeReview is not null)
            return null;

        await context.Reviews.AddAsync(review);
        return review;
    }

    public async Task<ReviewEntity?> GetByReviewIdWithUserAndVideoAsync(Guid reviewId)
    {
        return await context.Reviews
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Video)
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
    }

    public async Task<ReviewEntity?> GetByUserAndVideoIdsWithUserAndVideoAsync(Guid userId, Guid videoId)
    {
        return await context.Reviews
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Video)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.VideoId == videoId);
    }

    public void Delete(ReviewEntity review)
    {
        context.Reviews.Remove(review);
    }

    public async Task<IEnumerable<ReviewEntity>> GetAllWithUserAndVideoAsync()
    {
        return await context.Reviews
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Video)
            .ToListAsync();
    }

    public void Update(ReviewEntity review)
    {
        context.Reviews.Update(review);
    }
}