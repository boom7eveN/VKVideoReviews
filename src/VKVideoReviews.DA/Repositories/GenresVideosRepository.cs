using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class GenresVideosRepository(VkVideoReviewsDbContext context) : IGenresVideosRepository
{
    public async Task AddRangeAsync(IEnumerable<GenresVideosEntity> entities)
    {
        await context.GenresVideosEntities.AddRangeAsync(entities);
    }

    public async Task DeleteByVideoIdAsync(Guid videoId)
    {
        var entities = await context.GenresVideosEntities
            .Where(x => x.VideoId == videoId)
            .ToListAsync();

        context.GenresVideosEntities.RemoveRange(entities);
    }
}