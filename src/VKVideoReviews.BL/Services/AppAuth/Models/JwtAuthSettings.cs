namespace VKVideoReviews.BL.Services.AppAuth.Models;

public class JwtAuthSettings
{
    public string Secret { get; set; } = String.Empty;
    public string Issuer { get; set; } = String.Empty;
    public string Audience { get; set; } = String.Empty;
    public int AccessTokenLifeTimeMinutes { get; set; } = 60;
    public int RefreshTokenLifeTimeDays { get; set; } = 180;
}