using VKVideoReviews.BL.Services.AppAuth.Models;

namespace VKVideoReviews.WebApi.Settings;

public static class AppSettingsReader
{
    public static AppSettings Read(IConfiguration configuration)
    {
        return new AppSettings()
        {
            VkVideoReviewsDbConnectionString = configuration.GetSection("ConnectionStrings:VkVideoReviews").Value,
            ProtectedKey = configuration.GetSection("VkKeys:ProtectedKey").Value,
            ServiceKey = configuration.GetSection("VkKeys:ServiceKey").Value,
            ClientId = configuration.GetSection("VkKeys:ClientId").Value,
            RedirectUri = configuration.GetSection("RedirectUri").Value,
            VkIdUri = configuration.GetSection("VkApiUris:VkId").Value,
            VkMethodsUri = configuration.GetSection("VkApiUris:VkMethods").Value,
            JwtAuthSettings = new JwtAuthSettings
            {
                Secret = configuration["Jwt:Secret"] ?? string.Empty,
                Issuer = configuration["Jwt:Issuer"] ?? "VKVideoReviews",
                Audience = configuration["Jwt:Audience"] ?? "VKVideoReviews",
                AccessTokenLifeTimeMinutes = int.TryParse(configuration["Jwt:AccessTokenLifetimeMinutes"], out var m)
                    ? m
                    : 60,
                RefreshTokenLifeTimeDays = int.TryParse(configuration["Jwt:RefreshTokenLifetimeDays"], out var d)
                    ? d
                    : 180,
            },
            AdminVkUserIds = configuration.GetSection("AdminVkUserIds").Get<long[]>() ?? [],
            EncryptionKey = configuration.GetSection("EncryptionKey").Value,
        };
    }
}