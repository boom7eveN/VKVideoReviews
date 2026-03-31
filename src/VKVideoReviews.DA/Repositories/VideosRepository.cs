using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class VideosRepository(VkVideoReviewsDbContext context) : IVideosRepository
{
    public async Task<VideoEntity?> CreateAsync(VideoEntity entity)
    {
        var maybeVideo = await context.Videos.FirstOrDefaultAsync(
            x => 
                 x.Title == entity.Title && 
                 x.StartYear == entity.StartYear &&
                 x.VideoTypeId == entity.VideoTypeId);

        if (maybeVideo is not null)
            return null;
        var result = await context.Videos.AddAsync(entity);
        return result.Entity;
    }

    public async Task<IEnumerable<VideoEntity>> GetAllAsync()
    {
        return await context.Videos.AsNoTracking().ToListAsync();
    }

    public async Task<VideoEntity?> GetByIdAsync(Guid id)
    {
        return await context.Videos.AsNoTracking().FirstOrDefaultAsync(x => x.VideoId == id);
    }

    public void Delete(VideoEntity entity)
    {
        context.Videos.Remove(entity);
    }

    public void Update(VideoEntity entity)
    {
        context.Videos.Update(entity);
    }

    public async Task<VideoEntity?> GetVideoByIdWithGenresAndVideotypesAsync(Guid id)
    {
        return await context.Videos
            .AsNoTracking()
            .Include(v=> v.VideoType)
            .Include(v=>v.GenresVideos)
            .ThenInclude(gv => gv.Genre)
            .FirstOrDefaultAsync(v => v.VideoId == id);
    }

    public async Task<IEnumerable<VideoEntity>> GetAllVideosWithGenresAndVideotypesAsync()
    {
        return await context.Videos
            .AsNoTracking()
            .Include(v => v.VideoType)
            .Include(v => v.GenresVideos)
            .ThenInclude(gv => gv.Genre).ToListAsync();
    }
    
}