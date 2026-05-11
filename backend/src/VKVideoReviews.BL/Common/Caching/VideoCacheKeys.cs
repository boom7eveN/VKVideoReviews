namespace VKVideoReviews.BL.Common.Caching;

public static class VideoCacheKeys
{
    public static string Video(Guid videoId) => $"video:{videoId}";
}
