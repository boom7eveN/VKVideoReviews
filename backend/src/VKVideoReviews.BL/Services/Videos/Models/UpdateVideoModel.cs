namespace VKVideoReviews.BL.Services.Videos.Models;

public class UpdateVideoModel
{
    public string? VideoUrl { get; set; }
    public string? Title { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public Guid? VideoTypeId { get; set; }
    public List<Guid>? GenreIds { get; set; }
}