using System.ComponentModel.DataAnnotations.Schema;

namespace VKVideoReviews.DA.Entities;

[Table("Users")]
public class UserEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public long VkUserId { get; set; }
    public bool IsAdmin { get; set; } = false;
    public string AvatarUrl { get; set; }
    
    public virtual ICollection<ReviewEntity> Reviews { get; set; }
    public virtual ICollection<FavoriteEntity> Favorites { get; set; }
}