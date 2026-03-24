using System.Text.Json.Serialization;

namespace VKVideoReviews.BL.Services.AppAuth.Models;

public class VkApiResponse<T>
{
    [JsonPropertyName("response")]
    public List<T> Response { get; set; }
}