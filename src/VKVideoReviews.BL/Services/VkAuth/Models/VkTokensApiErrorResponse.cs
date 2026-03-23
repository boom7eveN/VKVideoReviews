using System.Text.Json.Serialization;

namespace VKVideoReviews.BL.Services.VkAuth.Models;

public class VkTokensApiErrorResponse
{
    [JsonPropertyName("error")] public string Error { get; set; }
    
    [JsonPropertyName("error_description")] public string ErrorDescription { get; set; }
}