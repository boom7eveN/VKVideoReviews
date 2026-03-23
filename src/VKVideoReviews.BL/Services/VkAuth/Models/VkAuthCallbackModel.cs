namespace VKVideoReviews.BL.Services.VkAuth.Models;

public class VkAuthCallbackModel
{
    public string Code { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
}