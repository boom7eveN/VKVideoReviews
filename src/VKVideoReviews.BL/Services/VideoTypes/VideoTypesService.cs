using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.VideoTypes.Interfaces;
using VKVideoReviews.BL.Services.VideoTypes.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.VideoTypes;

public class VideoTypesService(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IVideoTypesService
{
    public async Task<VideoTypeModel> CreateVideoTypeAsync(CreateVideoTypeModel model)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var videoTypeEntity = mapper.Map<VideoTypeEntity>(model);
            videoTypeEntity.VideoTypeId = Guid.NewGuid();

            videoTypeEntity = await unitOfWork.VideoTypes.CreateAsync(videoTypeEntity);

            if (videoTypeEntity is null)
                throw new AlreadyExistsException("VideoType");

            await unitOfWork.CommitAsync();
            return mapper.Map<VideoTypeModel>(videoTypeEntity);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            await unitOfWork.RollbackAsync();
            throw new AlreadyExistsException("VideoType");
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<VideoTypeModel>> GetAllVideoTypesAsync()
    {
        var videoTypes = await unitOfWork.VideoTypes.GetAllAsync();
        return mapper.Map<IEnumerable<VideoTypeModel>>(videoTypes);
    }

    public async Task<VideoTypeModel> GetVideoTypeByIdAsync(Guid id)
    {
        var videoType = await unitOfWork.VideoTypes.GetByIdAsync(id);
        if (videoType is null)
            throw new NotFoundException("VideoType", id);

        return mapper.Map<VideoTypeModel>(videoType);
    }

    public async Task<VideoTypeModel> UpdateVideoTypeAsync(Guid id, UpdateVideoTypeModel model)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var videoType = await unitOfWork.VideoTypes.GetByIdAsync(id);
            if (videoType is null)
                throw new NotFoundException("VideoType", id);

            if (!string.Equals(videoType.Title, model.Title, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await unitOfWork.VideoTypes.GetByTitleAsync(model.Title);
                if (existing is not null && existing.VideoTypeId != id)
                {
                    throw new AlreadyExistsException("VideoType");
                }
            }

            mapper.Map(model, videoType);
            unitOfWork.VideoTypes.Update(videoType);

            await unitOfWork.CommitAsync();
            return mapper.Map<VideoTypeModel>(videoType);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            await unitOfWork.RollbackAsync();
            throw new AlreadyExistsException("VideoType");
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteVideoTypeAsync(Guid id)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var videoType = await unitOfWork.VideoTypes.GetByIdAsync(id);
            if (videoType is null)
                throw new NotFoundException("VideoType", id);

            unitOfWork.VideoTypes.Delete(videoType);
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}