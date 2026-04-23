using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class FavoriteRepository(VkVideoReviewsDbContext context) : IFavoriteRepository
{
    public async Task<FavoriteEntity> CreateFavoriteAsync(FavoriteEntity favorite)
    {
        await context.Favorite.AddAsync(favorite);
        return favorite;
    }

    public async Task<FavoriteEntity?> GetFavoriteWithVideoAsync(Guid userId, Guid videoId)
    {
        return await context.Favorite.AsNoTracking()
            .Include(f => f.Video)
            .ThenInclude(v => v.VideoType)
            .Include(f => f.Video)
            .ThenInclude(v => v.GenresVideos)
            .ThenInclude(gv => gv.Genre)
            .FirstOrDefaultAsync(f =>
                f.VideoId == videoId &&
                f.UserId == userId);
    }

    public async Task<IEnumerable<FavoriteEntity>> GetAllFavoriteByUserIdWithVideoAsync(Guid userId)
    {
        return await context.Favorite.AsNoTracking()
            .Include(f => f.Video)
            .ThenInclude(v => v.VideoType)
            .Include(f => f.Video)
            .ThenInclude(v => v.GenresVideos)
            .ThenInclude(gv => gv.Genre)
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }

    public async Task<(IReadOnlyList<FavoriteEntity> Items, int TotalCount)> GetFavoritesByUserPagedWithVideoAsync(
        Guid userId,
        int pageNumber,
        int pageSize)
    {
        var query = context.Favorite
            .AsNoTracking()
            .Where(f => f.UserId == userId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(f => f.CreateDate)
            .ThenBy(f => f.VideoId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(f => f.Video)
            .ThenInclude(v => v.VideoType)
            .Include(f => f.Video)
            .ThenInclude(v => v.GenresVideos)
            .ThenInclude(gv => gv.Genre)
            .AsSplitQuery()
            .ToListAsync();

        return (items, totalCount);
    }

    public void DeleteFavorite(FavoriteEntity favorite)
    {
        context.Favorite.Remove(favorite);
    }

    public async Task<FavoriteEntity?> GetFavoriteByUserAndVideoIdsAsync(Guid userId, Guid videoId)
    {
        return await context.Favorite.AsNoTracking().FirstOrDefaultAsync(f =>
            f.VideoId == videoId &&
            f.UserId == userId);
    }
}