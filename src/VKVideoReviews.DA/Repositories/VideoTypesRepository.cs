using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class VideoTypesRepository(VkVideoReviewsDbContext context) : IVideoTypesRepository
{
    public async Task<VideoTypeEntity?> CreateAsync(VideoTypeEntity entity)
    {
        var maybeType = await context.VideoTypes.FirstOrDefaultAsync(x => x.Title == entity.Title);
        if (maybeType is not null)
            return null;

        var result = await context.VideoTypes.AddAsync(entity);
        return result.Entity;
    }

    public async Task<IEnumerable<VideoTypeEntity>> GetAllAsync()
    {
        return await context.VideoTypes.AsNoTracking().ToListAsync();
    }

    public async Task<VideoTypeEntity?> GetByIdAsync(Guid id)
    {
        return await context.VideoTypes.AsNoTracking().FirstOrDefaultAsync(x => x.VideoTypeId == id);
    }

    public void Delete(VideoTypeEntity entity)
    {
        context.VideoTypes.Remove(entity);
    }

    public void Update(VideoTypeEntity entity)
    {
        context.VideoTypes.Update(entity);
    }

    public async Task<VideoTypeEntity?> GetVideoTypeByTitleAsync(string title)
    {
        return await context.VideoTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Title == title);
    }
}