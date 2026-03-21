using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class VideoTypesRepository(VkVideoReviewsDbContext context) : IVideoTypesRepository
{
    public async Task<VideoTypeEntity?> CreateAsync(VideoTypeEntity entity)
    {
        var maybeType = await context.Genres.FirstOrDefaultAsync(x => x.GenreId == entity.VideoTypeId);
        if (maybeType is not null)
            return null;

        await context.VideoTypes.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<VideoTypeEntity>> GetAllAsync()
    {
        return await context.VideoTypes.AsNoTracking().ToListAsync();
    }

    public async Task<VideoTypeEntity?> GetByIdAsync(Guid id)
    {
        return await context.VideoTypes.AsNoTracking().FirstOrDefaultAsync(x => x.VideoTypeId == id);
    }

    public async Task DeleteAsync(VideoTypeEntity entity)
    {
        context.VideoTypes.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<VideoTypeEntity> UpdateAsync(VideoTypeEntity entity)
    {
        var result = context.VideoTypes.Attach(entity);
        result.State = EntityState.Modified;
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<VideoTypeEntity?> GetByTitleAsync(string title)
    {
        return await context.VideoTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Title == title);
    }
}