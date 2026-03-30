using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.Genres.Interfaces;
using VKVideoReviews.BL.Services.Genres.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.Genres;

public class GenresService(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IGenresService
{
    public async Task<GenreModel> CreateGenreAsync(CreateGenreModel model)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var genreEntity = mapper.Map<GenreEntity>(model);
            genreEntity.GenreId = Guid.NewGuid();

            genreEntity = await unitOfWork.Genres.CreateAsync(genreEntity);

            if (genreEntity is null)
                throw new AlreadyExistsException("Genre");

            await unitOfWork.CommitAsync();
            return mapper.Map<GenreModel>(genreEntity);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            await unitOfWork.RollbackAsync();
            throw new AlreadyExistsException("Genre");
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<GenreModel>> GetAllGenresAsync()
    {
        var genres = await unitOfWork.Genres.GetAllAsync();
        return mapper.Map<IEnumerable<GenreModel>>(genres);
    }

    public async Task<GenreModel> GetGenreByIdAsync(Guid id)
    {
        var genre = await unitOfWork.Genres.GetByIdAsync(id);
        if (genre is null)
            throw new NotFoundException("Genre", id);

        return mapper.Map<GenreModel>(genre);
    }

    public async Task<GenreModel> UpdateGenreAsync(Guid id, UpdateGenreModel model)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var genre = await unitOfWork.Genres.GetByIdAsync(id);
            if (genre is null)
                throw new NotFoundException("Genre", id);

            if (!string.Equals(genre.Title, model.Title, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await unitOfWork.Genres.GetByTitleAsync(model.Title);
                if (existing is not null && existing.GenreId != id)
                {
                    throw new AlreadyExistsException("Genre");
                }
            }

            mapper.Map(model, genre);
            unitOfWork.Genres.Update(genre);

            await unitOfWork.CommitAsync();
            return mapper.Map<GenreModel>(genre);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            await unitOfWork.RollbackAsync();
            throw new AlreadyExistsException("Genre");
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteGenreAsync(Guid id)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var genre = await unitOfWork.Genres.GetByIdAsync(id);
            if (genre is null)
                throw new NotFoundException("Genre", id);

            unitOfWork.Genres.Delete(genre);
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}