namespace VKVideoReviews.WebApi.Controllers.Responses.AppAuth;

public class AuthTokensResponse
{
    public string AccessToken { get; set; } = String.Empty;
    public string RefreshToken { get; set; } = String.Empty;
    public int ExpiresInSeconds { get; set; }
    public string TokenType { get; set; } = "Bearer";
}