using VKVideoReviews.WebApi.Controllers.Responses.AppAuth;

namespace VKVideoReviews.WebApi.Controllers.Responses.Reviews;

public class ReviewSummaryResponse
{
    public Guid ReviewId { get; set; }
    public int Rate { get; set; }
    public string Text { get; set; }
    public UserResponse User { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}
