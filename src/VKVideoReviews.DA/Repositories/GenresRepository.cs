using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class GenresRepository(VkVideoReviewsDbContext context) : IGenresRepository
{
    public async Task<GenreEntity?> CreateAsync(GenreEntity entity)
    {
        var maybeGenre = await context.Genres.FirstOrDefaultAsync(x => x.GenreId == entity.GenreId);

        if (maybeGenre is not null)
            return null;

        await context.Genres.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<GenreEntity>> GetAllAsync()
    {
        return await context.Genres.AsNoTracking().ToListAsync();
    }

    public async Task<GenreEntity?> GetByIdAsync(Guid id)
    {
        return await context.Genres.AsNoTracking().FirstOrDefaultAsync(x => x.GenreId == id);
    }

    public async Task DeleteAsync(GenreEntity entity)
    {
        context.Genres.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<GenreEntity> UpdateAsync(GenreEntity entity)
    {
        var result = context.Genres.Attach(entity);
        result.State = EntityState.Modified;
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<GenreEntity?> GetByTitleAsync(string title)
    {
        return await context.Genres.AsNoTracking().FirstOrDefaultAsync(x => x.Title == title);
    }
}