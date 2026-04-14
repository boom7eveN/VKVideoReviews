using System.Text.Json;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.Genres.Interfaces;
using VKVideoReviews.BL.Services.Genres.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.Genres;

public class GenresService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateGenreModel> createValidator,
    IValidator<UpdateGenreModel> updateValidator,
    IDistributedCache cache)
    : IGenresService
{
    private const string AllGenresCacheKey = "genres:all";
    private static readonly TimeSpan AllGenresCacheTime = TimeSpan.FromHours(1);

    public async Task<GenreModel> CreateGenreAsync(CreateGenreModel createGenreModel)
    {
        await ValidateAsync(createValidator, createGenreModel);
        var genreEntity = mapper.Map<GenreEntity>(createGenreModel);
        genreEntity.GenreId = Guid.NewGuid();
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var maybegrenre = await unitOfWork.Genres.GetGenreByTitleAsync(createGenreModel.Title);

            if (maybegrenre is not null)
                throw new AlreadyExistsException("Genre");

            genreEntity = await unitOfWork.Genres.CreateGenreAsync(genreEntity);

            await unitOfWork.CommitAsync();
            await cache.RemoveAsync(AllGenresCacheKey);
            return mapper.Map<GenreModel>(genreEntity);
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<GenreModel>> GetAllGenresAsync()
    {
        var cached = await cache.GetStringAsync(AllGenresCacheKey);
        if (cached is not null)
        {
            return JsonSerializer.Deserialize<List<GenreModel>>(cached)!;
        }
        
        var entities = await unitOfWork.Genres.GetAllGenresAsync();
        var models = mapper.Map<List<GenreModel>>(entities);
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = AllGenresCacheTime
        };
        await cache.SetStringAsync(AllGenresCacheKey, JsonSerializer.Serialize(models), options);
        
        return models;
    }

    public async Task<GenreModel> GetGenreByIdAsync(Guid genreId)
    {
        var genre = await unitOfWork.Genres.GetGenreByIdAsync(genreId);
        if (genre is null)
            throw new NotFoundException("Genre", genreId);

        return mapper.Map<GenreModel>(genre);
    }

    public async Task<GenreModel> UpdateGenreAsync(Guid genreId, UpdateGenreModel updateGenreModel)
    {
        await ValidateAsync(updateValidator, updateGenreModel);
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var genre = await unitOfWork.Genres.GetGenreByIdAsync(genreId);
            if (genre is null)
                throw new NotFoundException("Genre", genreId);

            if (!string.Equals(genre.Title, updateGenreModel.Title, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await unitOfWork.Genres.GetGenreByTitleAsync(updateGenreModel.Title);
                if (existing is not null && existing.GenreId != genreId) throw new AlreadyExistsException("Genre");
            }

            mapper.Map(updateGenreModel, genre);
            unitOfWork.Genres.UpdateGenre(genre);

            await unitOfWork.CommitAsync();
            await cache.RemoveAsync(AllGenresCacheKey);
            return mapper.Map<GenreModel>(genre);
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteGenreAsync(Guid genreId)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var genre = await unitOfWork.Genres.GetGenreByIdAsync(genreId);
            if (genre is null)
                throw new NotFoundException("Genre", genreId);

            unitOfWork.Genres.DeleteGenre(genre);
            await unitOfWork.CommitAsync();
            await cache.RemoveAsync(AllGenresCacheKey);
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    private static async Task ValidateAsync<T>(IValidator<T> validator, T model)
    {
        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            throw new ModelValidationException(errors);
        }
    }
}