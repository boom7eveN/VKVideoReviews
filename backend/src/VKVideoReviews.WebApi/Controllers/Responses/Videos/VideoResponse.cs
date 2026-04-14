using VKVideoReviews.WebApi.Controllers.Responses.Genres;
using VKVideoReviews.WebApi.Controllers.Responses.Reviews;
using VKVideoReviews.WebApi.Controllers.Responses.VideoType;

namespace VKVideoReviews.WebApi.Controllers.Responses.Videos;

public class VideoResponse
{
    public Guid VideoId { get; set; }
    public string VideoUrl { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public int StartYear { get; set; }
    public int? EndYear { get; set; }
    public double AverageRate { get; set; } = 0.0;
    public int TotalReviews { get; set; } = 0;
    public VideoTypeResponse VideoType { get; set; }
    public List<GenreResponse> Genres { get; set; } = new();
    public List<ReviewResponse> Reviews { get; set; } = new();
}