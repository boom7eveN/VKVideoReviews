using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.BL.Services.Videos.Models;

namespace VKVideoReviews.BL.Services.Reviews.Models;

public class ReviewModel
{
    public Guid ReviewId { get; set; }
    public int Rate { get; set; }
    public string Text { get; set; }
    public UserModel User { get; set; }
    public VideoModel Video { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}