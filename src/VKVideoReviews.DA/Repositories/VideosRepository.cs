using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class VideosRepository(VkVideoReviewsDbContext context)  : IVideosRepository
{
    public async Task<VideoEntity?> CreateAsync(VideoEntity entity)
    {
        var maybeVideo = await context.Videos.FirstOrDefaultAsync(x => x.VideoId == entity.VideoId);

        if (maybeVideo is not null)
            return null;

        await context.Videos.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<VideoEntity>> GetAllAsync()
    {
        return await context.Videos.AsNoTracking().ToListAsync();
    }

    public async Task<VideoEntity?> GetByIdAsync(Guid id)
    {
        return await context.Videos.AsNoTracking().FirstOrDefaultAsync(x => x.VideoId == id);
    }

    public async Task DeleteAsync(VideoEntity entity)
    {
        context.Videos.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<VideoEntity> UpdateAsync(VideoEntity entity)
    {
        var result = context.Videos.Attach(entity);
        result.State = EntityState.Modified;
        await context.SaveChangesAsync();
        return result.Entity;
    }
}