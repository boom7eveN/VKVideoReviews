namespace VKVideoReviews.WebApi.Settings;

public class AppSettingsReader
{
    public AppSettings Read(IConfiguration configuration)
    {
        return new AppSettings()
        {
            ProtectedKey = configuration.GetSection("VKKeys:ProtectedKey").Value,
            ServiceKey = configuration.GetSection("VKKeys:ServiceKey").Value,
        };
    }
}