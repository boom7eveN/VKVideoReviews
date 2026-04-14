using System.Text.Json;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Services.Videos.Interfaces;
using VKVideoReviews.BL.Services.Videos.Models;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.UnitOfWork.Interfaces;

namespace VKVideoReviews.BL.Services.Videos;

public class VideosService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateVideoModel> createValidator,
    IValidator<UpdateVideoModel> updateValidator,
    IDistributedCache cache) : IVideosService
{
    internal static string VideoCacheKey(Guid videoId) => $"video:{videoId}";
    private static readonly TimeSpan VideoCacheTime = TimeSpan.FromMinutes(5);

    public async Task<VideoModel> CreateVideoAsync(CreateVideoModel createVideoModel)
    {
        await ValidateAsync(createValidator, createVideoModel);

        var video = mapper.Map<VideoEntity>(createVideoModel);
        video.VideoId = Guid.NewGuid();
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var videoType = await unitOfWork.VideoTypes.GetVideoTypeByIdAsync(video.VideoTypeId);
            if (videoType is null)
                throw new NotFoundException("VideoType", video.VideoTypeId);

            var existingVideo = await unitOfWork.Videos
                .GetVideoByTitleYearAndTypeAsync(video.Title, video.StartYear, video.VideoTypeId);
            if (existingVideo is not null)
                throw new AlreadyExistsException("Video with same title, year and type");

            video = await unitOfWork.Videos.CreateVideoAsync(video);

            var uniqueGenreIds = createVideoModel.GenreIds.Distinct().ToHashSet();
            var existingGenres = await unitOfWork.Genres
                .GetAllGenresAsync(g => uniqueGenreIds.Contains(g.GenreId));
            var genreEntities = existingGenres.ToList();
            if (genreEntities.Count != uniqueGenreIds.Count)
                throw new NotFoundException("Genre");
            var genresVideos = genreEntities.Select(genre =>
                new GenresVideosEntity
                {
                    VideoId = video.VideoId,
                    GenreId = genre.GenreId
                });
            await unitOfWork.GenresVideos.AddGenresVideosRangeAsync(genresVideos);
            await unitOfWork.CommitAsync();
            var videoWithRelations =
                await unitOfWork.Videos.GetVideoByIdWithGenresAndVideotypeAsync(video.VideoId);
            var result = mapper.Map<VideoModel>(videoWithRelations);
            await CacheVideoAsync(result);
            return result;
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
            await unitOfWork.Videos.GetAllVideosWithGenresAndVideotypeAsync();
        return mapper.Map<IEnumerable<VideoModel>>(videos);
    }

    public async Task<VideoModel> GetVideoByIdAsync(Guid videoId)
    {
        var cacheKey = VideoCacheKey(videoId);
        var cached = await cache.GetStringAsync(cacheKey);

        if (cached is not null)
        {
            return JsonSerializer.Deserialize<VideoModel>(cached)!;
        }

        var video = await unitOfWork.Videos.GetVideoByIdWithGenresVideotypeAndReviewsAsync(videoId);
        if (video is null)
            throw new NotFoundException("Video", videoId);

        var result = mapper.Map<VideoModel>(video);
        await CacheVideoAsync(result);

        return result;
    }

    public async Task<VideoModel> UpdateVideoAsync(Guid videoId, UpdateVideoModel updateVideoModel)
    {
        await ValidateAsync(updateValidator, updateVideoModel);

        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var video = await unitOfWork.Videos.GetVideoByIdAsync(videoId);
            if (video is null)
                throw new NotFoundException("Video", videoId);

            if (updateVideoModel.Title is not null)
                video.Title = updateVideoModel.Title;

            if (updateVideoModel.Description is not null)
                video.Description = updateVideoModel.Description;

            if (updateVideoModel.ImageUrl is not null)
                video.ImageUrl = updateVideoModel.ImageUrl;

            if (updateVideoModel.VideoUrl is not null)
                video.VideoUrl = updateVideoModel.VideoUrl;

            if (updateVideoModel.StartYear.HasValue)
                video.StartYear = updateVideoModel.StartYear.Value;

            if (updateVideoModel.EndYear != null)
                video.EndYear = updateVideoModel.EndYear;

            if (updateVideoModel.VideoTypeId.HasValue)
            {
                var videoType = await unitOfWork.VideoTypes
                    .GetVideoTypeByIdAsync(updateVideoModel.VideoTypeId.Value);

                if (videoType is null)
                    throw new NotFoundException("VideoType", updateVideoModel.VideoTypeId.Value);

                video.VideoTypeId = updateVideoModel.VideoTypeId.Value;
            }

            if (updateVideoModel.GenreIds is not null)
            {
                var uniqueGenreIds = updateVideoModel.GenreIds.Distinct().ToHashSet();

                var existingGenres = await unitOfWork.Genres
                    .GetAllGenresAsync(g => uniqueGenreIds.Contains(g.GenreId));

                var genreEntities = existingGenres.ToList();

                if (genreEntities.Count != uniqueGenreIds.Count)
                    throw new NotFoundException("Genre");

                await unitOfWork.GenresVideos.DeleteGenreVideoByVideoIdAsync(videoId);
                var newGenresVideos = genreEntities.Select(g => new GenresVideosEntity
                {
                    VideoId = videoId,
                    GenreId = g.GenreId
                });

                await unitOfWork.GenresVideos.AddGenresVideosRangeAsync(newGenresVideos);
            }

            unitOfWork.Videos.UpdateVideo(video);
            await unitOfWork.CommitAsync();

            var result = await unitOfWork.Videos.GetVideoByIdWithGenresVideotypeAndReviewsAsync(videoId);
            var updatedModel = mapper.Map<VideoModel>(result);

            await CacheVideoAsync(updatedModel);

            return updatedModel;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteVideoAsync(Guid videoId)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var video = await unitOfWork.Videos.GetVideoByIdAsync(videoId);
            if (video is null)
                throw new NotFoundException("Video", videoId);
            unitOfWork.Videos.DeleteVideo(video);
            await unitOfWork.CommitAsync();
            await cache.RemoveAsync(VideoCacheKey(videoId));
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task CacheVideoAsync(VideoModel video)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = VideoCacheTime
        };
        await cache.SetStringAsync(VideoCacheKey(video.VideoId), JsonSerializer.Serialize(video), options);
    }


    private async Task ValidateAsync<T>(IValidator<T> validator, T model)
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