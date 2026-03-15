namespace VKVideoReviews.WebApi.Settings;

public static class AppSettingsReader
{
    public static AppSettings Read(IConfiguration configuration)
    {
        return new AppSettings()
        {
            VkVideoReviewsDbConnectionString = configuration.GetSection("ConnectionStrings:VKVideoReviews").Value,
            ProtectedKey = configuration.GetSection("VKKeys:ProtectedKey").Value,
            ServiceKey = configuration.GetSection("VKKeys:ServiceKey").Value,
        };
    }
}