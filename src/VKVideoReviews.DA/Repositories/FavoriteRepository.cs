using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class FavoriteRepository(VkVideoReviewsDbContext context) : IFavoriteRepository
{
    public async Task<FavoriteEntity?> CreateFavoriteAsync(FavoriteEntity favorite)
    {
        var maybeFavorite = await context.Favorite
            .FirstOrDefaultAsync(f =>
                f.VideoId == favorite.VideoId &&
                f.UserId == favorite.UserId);
        if (maybeFavorite is not null)
            return null;
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

    public async Task<IEnumerable<FavoriteEntity>> GetAllFavoriteWithVideoAsync(Guid userId)
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

    public void DeleteFavorite(FavoriteEntity favorite)
    {
        context.Favorite.Remove(favorite);
    }

    public async Task<FavoriteEntity?> GetFavoriteAsync(Guid userId, Guid videoId)
    {
        return await context.Favorite.AsNoTracking().FirstOrDefaultAsync(f =>
            f.VideoId == videoId &&
            f.UserId == userId);
    }
}