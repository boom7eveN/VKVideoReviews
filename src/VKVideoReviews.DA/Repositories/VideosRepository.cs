using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class VideosRepository(VkVideoReviewsDbContext context) : IVideosRepository
{
    public async Task<VideoEntity?> CreateVideoAsync(VideoEntity videoEntity)
    {
        var maybeVideo = await context.Videos.FirstOrDefaultAsync(x =>
            x.Title == videoEntity.Title &&
            x.StartYear == videoEntity.StartYear &&
            x.VideoTypeId == videoEntity.VideoTypeId);

        if (maybeVideo is not null)
            return null;

        var result = await context.Videos.AddAsync(videoEntity);
        return result.Entity;
    }
    

    public async Task<VideoEntity?> GetVideoByIdAsync(Guid videoId)
    {
        return await context.Videos.AsNoTracking().FirstOrDefaultAsync(x => x.VideoId == videoId);
    }

    public void DeleteVideo(VideoEntity videoEntity)
    {
        context.Videos.Remove(videoEntity);
    }

    public void UpdateVideo(VideoEntity videoEntity)
    {
        context.Videos.Update(videoEntity);
    }

    public async Task<VideoEntity?> GetVideoByIdWithGenresAndVideotypeAsync(Guid videoId)
    {
        return await context.Videos
            .AsNoTracking()
            .Include(v => v.VideoType)
            .Include(v => v.GenresVideos)
            .ThenInclude(gv => gv.Genre)
            .FirstOrDefaultAsync(v => v.VideoId == videoId);
    }

    public async Task<IEnumerable<VideoEntity>> GetAllVideosWithGenresAndVideotypeAsync()
    {
        return await context.Videos
            .AsNoTracking()
            .Include(v => v.VideoType)
            .Include(v => v.GenresVideos)
            .ThenInclude(gv => gv.Genre).ToListAsync();
    }

    public async Task UpdateVideoRatingByIdAsync(Guid videoId)
    {
        await context.Database.ExecuteSqlRawAsync(@"
        UPDATE ""Videos""
        SET 
            ""TotalReviews"" = (
                SELECT COUNT(*) 
                FROM ""Reviews"" 
                WHERE ""VideoId"" = {0}
            ),
            ""AverageRate"" = (
                SELECT COALESCE(AVG(""Rate""::double precision), 0)
                FROM ""Reviews"" 
                WHERE ""VideoId"" = {0}
            )
        WHERE ""VideoId"" = {0}
    ", videoId);
    }

    public async Task<VideoEntity?> LockForUpdateByIdAsync(Guid videoId)
    {
        return await context.Videos
            .FromSqlRaw("SELECT * FROM \"Videos\" WHERE \"VideoId\" = {0} FOR UPDATE", videoId)
            .FirstOrDefaultAsync();
    }
}