using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace VKVideoReviews.WebApi.Controllers.Requests.VkAuth;

public class VkAuthCallbackRequest
{
    [FromQuery(Name = "code")] public string Code { get; set; } = string.Empty;

    [FromQuery(Name = "state")] public string State { get; set; } = string.Empty;

    [FromQuery(Name = "device_id")] public string DeviceId { get; set; } = string.Empty;
}