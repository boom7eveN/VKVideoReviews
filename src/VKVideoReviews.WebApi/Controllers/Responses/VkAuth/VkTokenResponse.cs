using System.Text.Json.Serialization;

namespace VKVideoReviews.WebApi.Controllers.Responses.VkAuth;

public class VkTokenResponse
{
    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; }

    [JsonPropertyName("access_token")] public string AccessToken { get; set; }

    [JsonPropertyName("id_token")] public string IdToken { get; set; }

    [JsonPropertyName("token_type")] public string TokenType { get; set; }

    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }

    [JsonPropertyName("user_id")] public long UserId { get; set; }

    [JsonPropertyName("state")] public string State { get; set; }

    [JsonPropertyName("scope")] public string Scope { get; set; }
}