namespace VKVideoReviews.WebApi.Controllers.Responses.AppAuth;

public class UserResponse
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
}