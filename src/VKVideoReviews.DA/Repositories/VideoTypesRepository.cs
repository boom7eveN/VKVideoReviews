using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class VideoTypesRepository(VkVideoReviewsDbContext context) : IVideoTypesRepository
{
    public async Task<VideoTypeEntity> CreateVideoTypeAsync(VideoTypeEntity videoTypeEntity)
    {
        var result = await context.VideoTypes.AddAsync(videoTypeEntity);
        return result.Entity;
    }

    public async Task<IEnumerable<VideoTypeEntity>> GetAllVideoTypesAsync()
    {
        return await context.VideoTypes.AsNoTracking().ToListAsync();
    }

    public async Task<VideoTypeEntity?> GetVideoTypeByIdAsync(Guid videoTypeId)
    {
        return await context.VideoTypes.AsNoTracking().FirstOrDefaultAsync(x => x.VideoTypeId == videoTypeId);
    }

    public void DeleteVideoType(VideoTypeEntity videoTypeEntity)
    {
        context.VideoTypes.Remove(videoTypeEntity);
    }

    public void UpdateVideoType(VideoTypeEntity videoTypeEntity)
    {
        context.VideoTypes.Update(videoTypeEntity);
    }

    public async Task<VideoTypeEntity?> GetVideoTypeByTitleAsync(string title)
    {
        return await context.VideoTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Title == title);
    }
}