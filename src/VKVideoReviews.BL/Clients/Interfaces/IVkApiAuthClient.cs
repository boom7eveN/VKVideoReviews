using VKVideoReviews.BL.Integrations.Vk.Contracts.Requests;
using VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;

namespace VKVideoReviews.BL.Clients.Interfaces;

public interface IVkApiAuthClient
{
    Task<VkTokensApiResponse> ExchangeCodeAsync(VkTokenExchangeRequest request);
}