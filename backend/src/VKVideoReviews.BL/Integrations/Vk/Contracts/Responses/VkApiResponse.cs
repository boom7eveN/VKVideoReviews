using System.Text.Json.Serialization;

namespace VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;

public class VkApiResponse<T>
{
    [JsonPropertyName("response")] public List<T> Response { get; set; } = [];
}