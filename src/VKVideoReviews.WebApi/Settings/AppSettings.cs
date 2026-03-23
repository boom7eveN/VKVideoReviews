namespace VKVideoReviews.WebApi.Settings;

public class AppSettings
{
    public string VkVideoReviewsDbConnectionString { get; set; }
    public string ProtectedKey { get; set; }
    public string ServiceKey { get; set; }

    public string RedirectUri { get; set; }
    public string ClientId { get; set; }

    public string VkIdUri { get; set; }
}