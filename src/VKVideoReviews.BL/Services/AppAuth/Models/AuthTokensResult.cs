namespace VKVideoReviews.BL.Services.AppAuth.Models;

public class AuthTokensResult
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresInSeconds { get; set; }
    public string TokenType { get; set; } = "Bearer";
}