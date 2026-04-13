using System.Text.Json.Serialization;

namespace VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;

public class VkTokensApiResponse
{
    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; } = string.Empty;

    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("id_token")] public string IdToken { get; set; } = string.Empty;

    [JsonPropertyName("token_type")] public string TokenType { get; set; } = string.Empty;

    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }

    [JsonPropertyName("user_id")] public long UserId { get; set; }

    [JsonPropertyName("state")] public string State { get; set; } = string.Empty;

    [JsonPropertyName("scope")] public string Scope { get; set; } = string.Empty;
}