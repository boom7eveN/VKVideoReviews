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
    public async Task<VideoModel> CreateVideoAsync(CreateVideoModel createVideoModel)
    {
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
            var videoWithRelations = await unitOfWork.Videos.GetVideoByIdWithGenresAndVideotypeAsync(
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
            await unitOfWork.Videos.GetAllVideosWithGenresAndVideotypeAsync();
        return mapper.Map<IEnumerable<VideoModel>>(videos);
    }

    public async Task<VideoModel> GetVideoByIdAsync(Guid videoId)
    {
        var video = await unitOfWork.Videos.GetVideoByIdWithGenresAndVideotypeAsync(videoId);
        if (video is null)
            throw new NotFoundException("Video", videoId);
        return mapper.Map<VideoModel>(video);
    }

    public async Task<VideoModel> UpdateVideoAsync(Guid videoId, UpdateVideoModel updateVideoModel)
    {
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

            var result = await unitOfWork.Videos
                .GetVideoByIdWithGenresAndVideotypeAsync(videoId);

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
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}