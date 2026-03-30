using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class GenresRepository(VkVideoReviewsDbContext context) : IGenresRepository
{
    public async Task<GenreEntity?> CreateAsync(GenreEntity entity)
    {
        var maybeGenre = await context.Genres.FirstOrDefaultAsync(x => x.Title == entity.Title);

        if (maybeGenre is not null)
            return null;

        var result = await context.Genres.AddAsync(entity);
        return result.Entity;
    }

    public async Task<IEnumerable<GenreEntity>> GetAllAsync()
    {
        return await context.Genres.AsNoTracking().ToListAsync();
    }

    public async Task<GenreEntity?> GetByIdAsync(Guid id)
    {
        return await context.Genres.AsNoTracking().FirstOrDefaultAsync(x => x.GenreId == id);
    }

    public void Delete(GenreEntity entity)
    {
        context.Genres.Remove(entity);
    }

    public void Update(GenreEntity entity)
    {
        context.Genres.Update(entity);
    }

    public async Task<GenreEntity?> GetByTitleAsync(string title)
    {
        return await context.Genres.AsNoTracking().FirstOrDefaultAsync(x => x.Title == title);
    }
}