using AutoMapper;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.Genres.Interfaces;
using VKVideoReviews.BL.Services.Genres.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.BL.Services.Genres;

public class GenresService(IGenresRepository repository, IMapper mapper) : IGenresService
{
    public async Task<GenreModel> CreateGenreAsync(CreateGenreModel model)
    {
        var maybeGenre = await repository.GetByTitleAsync(model.Title);
        if (maybeGenre is not null)
            throw new AlreadyExistsException("Genre");

        var genreEntity = mapper.Map<GenreEntity>(model);
        genreEntity.GenreId = Guid.NewGuid();
        genreEntity = await repository.CreateAsync(genreEntity);
        if (genreEntity is null)
            throw new AlreadyExistsException("Genre");
        return mapper.Map<GenreModel>(genreEntity);
    }

    public async Task<IEnumerable<GenreModel>> GetAllGenresAsync()
    {
        var genres = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<GenreModel>>(genres);
    }

    public async Task<GenreModel> GetGenreByIdAsync(Guid id)
    {
        var maybeGenre = await repository.GetByIdAsync(id);
        if (maybeGenre is null)
            throw new NotFoundException("Genre");

        return mapper.Map<GenreModel>(maybeGenre);
    }

    public async Task<GenreModel> UpdateGenreAsync(Guid id, UpdateGenreModel model)
    {
        var maybeGenre = await repository.GetByIdAsync(id);
        if (maybeGenre is null)
            throw new NotFoundException("Genre", id);

        if (!string.Equals(maybeGenre.Title, model.Title, StringComparison.OrdinalIgnoreCase))
        {
            var genreWithSameTitle = await repository.GetByTitleAsync(model.Title);
            if (genreWithSameTitle is not null)
                throw new AlreadyExistsException("Genre with this title");
        }

        mapper.Map(model, maybeGenre);
        var updatedGenre = await repository.UpdateAsync(maybeGenre);

        return mapper.Map<GenreModel>(updatedGenre);
    }

    public async Task DeleteGenreAsync(Guid id)
    {
        var maybeGenre = await repository.GetByIdAsync(id);
        if (maybeGenre is null)
            throw new NotFoundException("Genre", id);

        await repository.DeleteAsync(maybeGenre);
    }
}