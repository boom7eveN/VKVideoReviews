using System.ComponentModel.DataAnnotations.Schema;

namespace VKVideoReviews.DA.Entities;

[Table("UserTokens")]
public class UserTokenEntity
{
    public Guid TokenRecordId { get; set; }

    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; }

    public long VkUserId { get; set; }
    
    public string VkAccessTokenEncrypted { get; set; } // 1h
    
    public string VkRefreshTokenEncrypted { get; set; } // 180d


    public DateTime AccessTokenExpiresAt { get; set; } //1h

    public DateTime RefreshTokenExpiresAt { get; set; } //180d


    public DateTime CreatedAt { get; set; }
}