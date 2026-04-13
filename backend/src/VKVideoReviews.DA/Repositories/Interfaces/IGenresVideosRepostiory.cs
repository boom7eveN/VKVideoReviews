using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IGenresVideosRepository
{
    Task AddGenresVideosRangeAsync(IEnumerable<GenresVideosEntity> genresVideos);
    Task DeleteGenreVideoByVideoIdAsync(Guid videoId);
}