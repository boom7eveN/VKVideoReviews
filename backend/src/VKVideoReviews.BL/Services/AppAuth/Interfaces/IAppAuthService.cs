using VKVideoReviews.BL.Integrations.Vk.Contracts.Responses;
using VKVideoReviews.BL.Services.AppAuth.Models;

namespace VKVideoReviews.BL.Services.AppAuth.Interfaces;

public interface IAppAuthService
{
    Task<AuthTokensModel> SignInWithVkTokensAsync(VkTokensApiResponse vkTokens);
    Task<AuthTokensModel?> RefreshAsync(string refreshToken);
    Task<UserModel> GetUserByIdAsync(Guid userId);
}