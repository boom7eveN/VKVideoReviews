using VKVideoReviews.WebApi.Controllers.Responses.AppAuth;
using VKVideoReviews.WebApi.Controllers.Responses.Videos;

namespace VKVideoReviews.WebApi.Controllers.Responses.Reviews;

public class ReviewResponse
{
    public Guid ReviewId { get; set; }
    public int Rate { get; set; }
    public String Text { get; set; }
    public UserResponse User { get; set; }
    public VideoResponse Video { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}