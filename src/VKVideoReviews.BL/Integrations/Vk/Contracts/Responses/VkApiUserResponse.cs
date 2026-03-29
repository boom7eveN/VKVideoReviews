using System.Text.Json.Serialization;

namespace VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;

public class VkApiUserResponse
{
    [JsonPropertyName("id")] public long VkUserId { get; set; }

    [JsonPropertyName("first_name")] public string Name { get; set; } = String.Empty;

    [JsonPropertyName("last_name")] public string Surname { get; set; } = String.Empty;

    [JsonPropertyName("photo_200")] public string AvatarUrl { get; set; } = String.Empty;
}