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
        };
    }
}