namespace VKVideoReviews.WebApi.Controllers.Responses.AppAuth;

public class UserResponse
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string AvatarUrl { get; set; }
}