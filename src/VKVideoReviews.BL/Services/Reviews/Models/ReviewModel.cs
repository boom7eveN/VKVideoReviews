using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.BL.Services.Videos.Models;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Services.Reviews.Models;

public class ReviewModel
{
    public Guid ReviewId { get; set; }
    public int Rate { get; set; }
    public String Text { get; set; }
    public UserModel User { get; set; }
    public VideoModel Video { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}