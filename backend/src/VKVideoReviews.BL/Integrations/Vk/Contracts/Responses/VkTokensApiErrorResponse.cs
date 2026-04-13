using System.Text.Json.Serialization;

namespace VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;

public class VkTokensApiErrorResponse
{
    [JsonPropertyName("error")] public string Error { get; set; } = string.Empty;

    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; set; } = string.Empty;
}