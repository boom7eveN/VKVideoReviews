using AutoMapper;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.VideoTypes.Interfaces;
using VKVideoReviews.BL.Services.VideoTypes.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.BL.Services.VideoTypes;

public class VideoTypesService(IVideoTypesRepository repository, IMapper mapper) : IVideoTypesService
{
    public async Task<VideoTypeModel> CreateVideoTypeAsync(CreateVideoTypeModel model)
    {
        var maybeType = await repository.GetByTitleAsync(model.Title);
        if (maybeType is not null)
            throw new AlreadyExistsException("VideoType");

        var typeEntity = mapper.Map<VideoTypeEntity>(model);
        typeEntity.VideoTypeId = Guid.NewGuid();
        typeEntity = await repository.CreateAsync(typeEntity);
        if (typeEntity is null)
            throw new AlreadyExistsException("VideoType");
        return mapper.Map<VideoTypeModel>(typeEntity);
    }

    public async Task<IEnumerable<VideoTypeModel>> GetAllVideoTypesAsync()
    {
        var videoTypes = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<VideoTypeModel>>(videoTypes);
    }

    public async Task<VideoTypeModel> GetVideoTypeByIdAsync(Guid id)
    {
        var maybeType = await repository.GetByIdAsync(id);
        if (maybeType is null)
            throw new NotFoundException("VideoType");

        return mapper.Map<VideoTypeModel>(maybeType);
    }

    public async Task<VideoTypeModel> UpdateVideoTypeAsync(Guid id, UpdateVideoTypeModel model)
    {
        var maybeType = await repository.GetByIdAsync(id);
        if (maybeType is null)
            throw new NotFoundException("VideoType", id);

        if (!string.Equals(maybeType.Title, model.Title, StringComparison.OrdinalIgnoreCase))
        {
            var typeWithSameTitle = await repository.GetByTitleAsync(model.Title);
            if (typeWithSameTitle is not null)
                throw new AlreadyExistsException("VideoType with this title");
        }

        mapper.Map(model, maybeType);
        var updatedVideoType = await repository.Update(maybeType);

        return mapper.Map<VideoTypeModel>(updatedVideoType);
    }

    public async Task DeleteVideoTypeAsync(Guid id)
    {
        var maybeType = await repository.GetByIdAsync(id);
        if (maybeType is null)
            throw new NotFoundException("VideoType", id);

        await repository.Delete(maybeType);
    }
}