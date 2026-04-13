namespace VKVideoReviews.WebApi.Controllers.Requests.Reviews;

public class CreateReviewRequest
{
    public int Rate { get; set; }
    public string Text { get; set; }
}