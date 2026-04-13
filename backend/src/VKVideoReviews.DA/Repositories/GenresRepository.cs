using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class GenresRepository(VkVideoReviewsDbContext context) : IGenresRepository
{
    public async Task<GenreEntity> CreateGenreAsync(GenreEntity genreEntity)
    {
        var result = await context.Genres.AddAsync(genreEntity);
        return result.Entity;
    }

    public async Task<IEnumerable<GenreEntity>> GetAllGenresAsync()
    {
        return await context.Genres.AsNoTracking().ToListAsync();
    }

    public async Task<GenreEntity?> GetGenreByIdAsync(Guid genreId)
    {
        return await context.Genres.AsNoTracking().FirstOrDefaultAsync(x => x.GenreId == genreId);
    }

    public void DeleteGenre(GenreEntity genreEntity)
    {
        context.Genres.Remove(genreEntity);
    }

    public void UpdateGenre(GenreEntity genreEntity)
    {
        context.Genres.Update(genreEntity);
    }

    public async Task<GenreEntity?> GetGenreByTitleAsync(string title)
    {
        return await context.Genres.AsNoTracking().FirstOrDefaultAsync(x => x.Title == title);
    }

    public async Task<IEnumerable<GenreEntity>> GetAllGenresAsync(Expression<Func<GenreEntity, bool>> predicate)
    {
        return await context.Genres.AsNoTracking().Where(predicate).ToListAsync();
    }
}