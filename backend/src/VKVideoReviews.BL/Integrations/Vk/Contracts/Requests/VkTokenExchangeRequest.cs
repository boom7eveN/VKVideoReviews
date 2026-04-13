namespace VKVideoReviews.BL.Integrations.Vk.Contracts.Requests;

public class VkTokenExchangeRequest
{
    public required string Code { get; init; }
    public required string DeviceId { get; init; }
    public required string State { get; init; }
    public required string CodeVerifier { get; init; }
    public required string RedirectUri { get; init; }
    public required string ClientId { get; init; }
}