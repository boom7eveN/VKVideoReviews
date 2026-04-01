using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class GenresVideosRepository(VkVideoReviewsDbContext context) : IGenresVideosRepository
{
    public async Task AddGenresVideosRangeAsync(IEnumerable<GenresVideosEntity> genresVideos)
    {
        await context.GenresVideosEntities.AddRangeAsync(genresVideos);
    }

    public async Task DeleteGenreVideoByVideoIdAsync(Guid videoId)
    {
        var genresVideosEntities = await context.GenresVideosEntities
            .Where(x => x.VideoId == videoId)
            .ToListAsync();

        context.GenresVideosEntities.RemoveRange(genresVideosEntities);
    }
}