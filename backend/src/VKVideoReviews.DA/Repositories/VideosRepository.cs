using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class VideosRepository(VkVideoReviewsDbContext context) : IVideosRepository
{
    public async Task<VideoEntity> CreateVideoAsync(VideoEntity videoEntity)
    {
        var result = await context.Videos.AddAsync(videoEntity);
        return result.Entity;
    }

    public async Task<VideoEntity?> GetVideoByTitleYearAndTypeAsync(string title, int startYear, Guid videoTypeId)
    {
        return await context.Videos
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Title == title &&
                x.StartYear == startYear &&
                x.VideoTypeId == videoTypeId);
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

    public async Task<VideoEntity?> GetVideoByIdWithGenresVideotypeAndReviewsAsync(Guid videoId)
    {
        return await context.Videos
            .AsNoTracking()
            .Include(v => v.VideoType)
            .Include(v => v.Reviews)
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

    public async Task<(IReadOnlyList<VideoEntity> Items, int TotalCount)> GetVideosPagedWithGenresAndVideotypeAsync(
        int pageNumber,
        int pageSize,
        string? titlePart)
    {
        var query = context.Videos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(titlePart))
        {
            var pattern = $"%{EscapeLikePattern(titlePart)}%";
            query = query.Where(v => EF.Functions.ILike(v.Title, pattern, "\\"));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(v => v.AverageRate)
            .ThenBy(v => v.VideoId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(v => v.VideoType)
            .Include(v => v.GenresVideos)
            .ThenInclude(gv => gv.Genre)
            .AsSplitQuery()
            .ToListAsync();

        return (items, totalCount);
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

    public async Task<VideoEntity?> GetVideoByIdLockForUpdateAsync(Guid videoId)
    {
        return await context.Videos
            .FromSqlRaw("SELECT * FROM \"Videos\" WHERE \"VideoId\" = {0} FOR UPDATE", videoId)
            .FirstOrDefaultAsync();
    }

    private static string EscapeLikePattern(string input)
    {
        return input
            .Replace("\\", "\\\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_");
    }
}