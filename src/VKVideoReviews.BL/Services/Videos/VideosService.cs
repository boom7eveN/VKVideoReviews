using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.Videos.Interfaces;
using VKVideoReviews.BL.Services.Videos.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.Videos;

public class VideosService(IUnitOfWork unitOfWork, IMapper mapper) : IVideosService
{
    public async Task<VideoModel> CreateVideoAsync(CreateVideoModel model)
    {
        var video = mapper.Map<VideoEntity>(model);
        video.VideoId = Guid.NewGuid();
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var videoType = await unitOfWork.VideoTypes.GetByIdAsync(video.VideoTypeId);
            if (videoType is null)
                throw new NotFoundException("VideoType", video.VideoTypeId);
            video.VideoTypeId = model.VideoTypeId;

            video = await unitOfWork.Videos.CreateAsync(video);
            if (video is null)
                throw new AlreadyExistsException("Video");

            var uniqueGenreIds = model.GenreIds.Distinct().ToHashSet();
            var existingGenres = await unitOfWork.Genres
                .GetAllAsync(g => uniqueGenreIds.Contains(g.GenreId));
            var genreEntities = existingGenres.ToList();
            if (genreEntities.Count != uniqueGenreIds.Count)
                throw new NotFoundException("Genre");
            var genresVideos = genreEntities.Select(genre =>
                new GenresVideosEntity
                {
                    VideoId = video.VideoId,
                    GenreId = genre.GenreId
                });
            await unitOfWork.GenresVideos.AddRangeAsync(genresVideos);
            await unitOfWork.CommitAsync();
            var videoWithRelations = await unitOfWork.Videos.GetVideoByIdWithGenresAndVideotypesAsync(
                video.VideoId);
            return mapper.Map<VideoModel>(videoWithRelations);
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<VideoModel>> GetAllVideosAsync()
    {
        var videos =
            await unitOfWork.Videos.GetAllVideosWithGenresAndVideotypesAsync();
        return mapper.Map<IEnumerable<VideoModel>>(videos);
    }

    public async Task<VideoModel> GetVideoByIdAsync(Guid id)
    {
        var video = await unitOfWork.Videos.GetVideoByIdWithGenresAndVideotypesAsync(id);
        return mapper.Map<VideoModel>(video);
    }

    public async Task<VideoModel> UpdateVideoAsync(Guid id, UpdateVideoModel model)
{
    await using var transaction = await unitOfWork.BeginTransactionAsync();
    try
    {
        var video = await unitOfWork.Videos.GetByIdAsync(id);
        if (video is null)
            throw new NotFoundException("Video", id);
        
        if (model.Title is not null)
            video.Title = model.Title;

        if (model.Description is not null)
            video.Description = model.Description;

        if (model.ImageUrl is not null)
            video.ImageUrl = model.ImageUrl;

        if (model.VideoUrl is not null)
            video.VideoUrl = model.VideoUrl;

        if (model.StartYear.HasValue)
            video.StartYear = model.StartYear.Value;

        if (model.EndYear != null) 
            video.EndYear = model.EndYear;
        
        if (model.VideoTypeId.HasValue)
        {
            var videoType = await unitOfWork.VideoTypes
                .GetByIdAsync(model.VideoTypeId.Value);

            if (videoType is null)
                throw new NotFoundException("VideoType", model.VideoTypeId.Value);

            video.VideoTypeId = model.VideoTypeId.Value;
        }
        
        if (model.GenreIds is not null)
        {
            var uniqueGenreIds = model.GenreIds.Distinct().ToHashSet();

            var existingGenres = await unitOfWork.Genres
                .GetAllAsync(g => uniqueGenreIds.Contains(g.GenreId));

            var genreEntities = existingGenres.ToList();

            if (genreEntities.Count != uniqueGenreIds.Count)
                throw new NotFoundException("Genre");
            
            await unitOfWork.GenresVideos.DeleteByVideoIdAsync(id);
            var newGenresVideos = genreEntities.Select(g => new GenresVideosEntity
            {
                VideoId = id,
                GenreId = g.GenreId
            });

            await unitOfWork.GenresVideos.AddRangeAsync(newGenresVideos);
        }

        unitOfWork.Videos.Update(video);
        await unitOfWork.CommitAsync();

        var result = await unitOfWork.Videos
            .GetVideoByIdWithGenresAndVideotypesAsync(id);

        return mapper.Map<VideoModel>(result);
    }
    catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
    {
        await unitOfWork.RollbackAsync();
        throw new AlreadyExistsException("Video");
    }
    catch
    {
        await unitOfWork.RollbackAsync();
        throw;
    }
}

    public async Task DeleteVideoAsync(Guid id)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var video = await unitOfWork.Videos.GetVideoByIdWithGenresAndVideotypesAsync(id);
            if (video is null)
                throw new NotFoundException("Video", id);
            unitOfWork.Videos.Delete(video);
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}