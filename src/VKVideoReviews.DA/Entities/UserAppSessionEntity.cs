using System.ComponentModel.DataAnnotations.Schema;

namespace VKVideoReviews.DA.Entities;

[Table("UserAppSessions")]
public class UserAppSessionEntity
{
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; }

    /// <summary>SHA256 Base64</summary>
    public string RefreshTokenHash { get; set; }

    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
}