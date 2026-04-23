using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class ReviewsRepository(VkVideoReviewsDbContext context) : IReviewsRepository
{
    public async Task<ReviewEntity> CreateReviewAsync(ReviewEntity review)
    {
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

    public async Task<(IReadOnlyList<ReviewEntity> Items, int TotalCount)> GetReviewsPagedWithUsersAndVideosAsync(
        int pageNumber,
        int pageSize)
    {
        var query = context.Reviews.AsNoTracking();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(r => r.CreateDate)
            .ThenBy(r => r.ReviewId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.User)
            .Include(r => r.Video)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<ReviewEntity> Items, int TotalCount)> GetReviewsByVideoPagedWithUsersAsync(
        Guid videoId,
        int pageNumber,
        int pageSize)
    {
        var query = context.Reviews
            .AsNoTracking()
            .Where(r => r.VideoId == videoId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(r => r.CreateDate)
            .ThenBy(r => r.ReviewId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.User)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(IReadOnlyList<ReviewEntity> Items, int TotalCount)> GetReviewsByUserPagedWithVideosAsync(
        Guid userId,
        int pageNumber,
        int pageSize)
    {
        var query = context.Reviews
            .AsNoTracking()
            .Where(r => r.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(r => r.CreateDate)
            .ThenBy(r => r.ReviewId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.Video)
            .ThenInclude(v => v.VideoType)
            .ToListAsync();

        return (items, totalCount);
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