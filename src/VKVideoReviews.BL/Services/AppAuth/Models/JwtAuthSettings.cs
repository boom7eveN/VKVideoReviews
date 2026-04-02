namespace VKVideoReviews.BL.Services.AppAuth.Models;

public class JwtAuthSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenLifeTimeMinutes { get; set; } = 60;
    public int RefreshTokenLifeTimeDays { get; set; } = 180;
}