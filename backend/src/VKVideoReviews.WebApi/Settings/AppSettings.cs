using VKVideoReviews.BL.Services.AppAuth.Models;

namespace VKVideoReviews.WebApi.Settings;

public class AppSettings
{
    public string VkVideoReviewsDbConnectionString { get; set; }
    public string ProtectedKey { get; set; }
    public string ServiceKey { get; set; }

    public string RedirectUri { get; set; }
    public string ClientId { get; set; }

    public string VkIdUri { get; set; }
    public string VkMethodsUri { get; set; }

    public JwtAuthSettings JwtAuthSettings { get; set; } = new();
    public long[] AdminVkUserIds { get; set; } = [];

    public string EncryptionKey { get; set; }

    public string RedisConnectionString { get; set; }
    public string RedisInstanceName { get; set; }
}