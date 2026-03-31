namespace VKVideoReviews.BL.Services.AppAuth.Models;

public class UserModel
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public long VkUserId { get; set; }
    public bool IsAdmin { get; set; } = false;
    public string AvatarUrl { get; set; }
}