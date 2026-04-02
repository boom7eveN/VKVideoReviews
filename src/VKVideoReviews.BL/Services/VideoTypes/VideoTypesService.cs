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
    public async Task<VideoTypeModel> CreateVideoTypeAsync(CreateVideoTypeModel createVideoTypeModel)
    {
        var videoTypeEntity = mapper.Map<VideoTypeEntity>(createVideoTypeModel);
        videoTypeEntity.VideoTypeId = Guid.NewGuid();
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var maybeVideoType = await unitOfWork.VideoTypes.GetVideoTypeByTitleAsync(createVideoTypeModel.Title);
            if (maybeVideoType is not null)
                throw new AlreadyExistsException("VideoType");
            
            videoTypeEntity = await unitOfWork.VideoTypes.CreateVideoTypeAsync(videoTypeEntity);
            
            await unitOfWork.CommitAsync();
            return mapper.Map<VideoTypeModel>(videoTypeEntity);
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<VideoTypeModel>> GetAllVideoTypesAsync()
    {
        var videoTypes = await unitOfWork.VideoTypes.GetAllVideoTypesAsync();
        return mapper.Map<IEnumerable<VideoTypeModel>>(videoTypes);
    }

    public async Task<VideoTypeModel> GetVideoTypeByIdAsync(Guid videoTypeId)
    {
        var videoType = await unitOfWork.VideoTypes.GetVideoTypeByIdAsync(videoTypeId);
        if (videoType is null)
            throw new NotFoundException("VideoType", videoTypeId);

        return mapper.Map<VideoTypeModel>(videoType);
    }

    public async Task<VideoTypeModel> UpdateVideoTypeAsync(Guid videoTypeId, UpdateVideoTypeModel updateVideoTypeModel)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var videoType = await unitOfWork.VideoTypes.GetVideoTypeByIdAsync(videoTypeId);
            if (videoType is null)
                throw new NotFoundException("VideoType", videoTypeId);

            if (!string.Equals(videoType.Title, updateVideoTypeModel.Title, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await unitOfWork.VideoTypes.GetVideoTypeByTitleAsync(updateVideoTypeModel.Title);
                if (existing is not null && existing.VideoTypeId != videoTypeId)
                {
                    throw new AlreadyExistsException("VideoType");
                }
            }

            mapper.Map(updateVideoTypeModel, videoType);
            unitOfWork.VideoTypes.UpdateVideoType(videoType);

            await unitOfWork.CommitAsync();
            return mapper.Map<VideoTypeModel>(videoType);
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteVideoTypeAsync(Guid videoTypeId)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var videoType = await unitOfWork.VideoTypes.GetVideoTypeByIdAsync(videoTypeId);
            if (videoType is null)
                throw new NotFoundException("VideoType", videoTypeId);

            unitOfWork.VideoTypes.DeleteVideoType(videoType);
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}