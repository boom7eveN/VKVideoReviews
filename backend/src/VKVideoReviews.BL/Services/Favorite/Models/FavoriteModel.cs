using VKVideoReviews.BL.Services.Videos.Models;

namespace VKVideoReviews.BL.Services.Favorite.Models;

public class FavoriteModel
{
    public Guid UserId { get; set; }
    public Guid VideoId { get; set; }
    public VideoModel Video { get; set; }
}