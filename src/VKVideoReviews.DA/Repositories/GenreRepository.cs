using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class GenreRepository(VkVideoReviewsDbContext context) : IGenresRepository
{
    public async Task<GenreEntity?> CreateAsync(GenreEntity entity)
    {
        var maybeGenre = await context.Genres.FirstOrDefaultAsync(x=> x.GenreId ==  entity.GenreId);

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

    public async Task Delete(GenreEntity entity)
    {
        context.Genres.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<GenreEntity> Update(GenreEntity entity)
    {
        var result = context.Genres.Attach(entity);
        result.State = EntityState.Modified;
        await context.SaveChangesAsync();
        return result.Entity;
    }
}