using System.Text.Json.Serialization;

namespace VKVideoReviews.BL.Services.AppAuth.Models;

public class VkApiUserResponse
{
    [JsonPropertyName("id")] public long VkUserId { get; set; }

    [JsonPropertyName("first_name")] public string Name { get; set; }

    [JsonPropertyName("last_name")] public string Surname { get; set; }

    [JsonPropertyName("photo_200")] public string AvatarUrl { get; set; }
}