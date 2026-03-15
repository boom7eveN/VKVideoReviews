using System.ComponentModel.DataAnnotations.Schema;

namespace VKVideoReviews.DA.Entities;

[Table("Favorite")]
public class FavoriteEntity
{
    public Guid VideoId { get; set; }
    public VideoEntity Video { get; set; }

    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
}