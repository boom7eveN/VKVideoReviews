namespace VKVideoReviews.BL.Integrations.Vk.Contracts.Requests;

public class VkUserGetRequest
{
    public required long UserId { get; init; }
    public required string AccessToken { get; init; }
    public string Fields { get; init; } = "photo_200";
    public string Version { get; init; } = "5.131";
}