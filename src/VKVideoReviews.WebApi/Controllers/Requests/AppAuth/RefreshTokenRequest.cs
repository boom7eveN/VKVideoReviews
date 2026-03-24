namespace VKVideoReviews.WebApi.Controllers.Requests.AppAuth;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}