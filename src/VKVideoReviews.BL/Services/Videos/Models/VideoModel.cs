using VKVideoReviews.BL.Services.Genres.Models;
using VKVideoReviews.BL.Services.VideoTypes.Models;

namespace VKVideoReviews.BL.Services.Videos.Models;

public class VideoModel
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
    public VideoTypeModel VideoType { get; set; }
    public List<GenreModel> Genres { get; set; } = new();
}