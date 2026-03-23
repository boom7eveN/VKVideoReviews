using AutoMapper;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.BL.Services.VkAuth.Models;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Services.AppAuth;

public class AppAuthService(IVkApiMethodsClient httpClient, IMapper mapper) : IAppAuthService
{
    public async Task<AuthTokensResult> SignInWithVkTokensAsync(VkTokensApiResponse vkTokens)
    {
        var userGetParams = GetParametersForUserGet(vkTokens);
        var userResponse = await httpClient.GetUserAsync(userGetParams);
        
        var userEntity = mapper.Map<UserEntity>(userResponse);
        userEntity.UserId = Guid.NewGuid();
        
        
        
    }
    
    private string GetParametersForUserGet(VkTokensApiResponse vkTokens)
    {
        var requestParams = new Dictionary<string, string>()
        {
            { "access_token", $"{vkTokens.AccessToken}" },
            { "fields", "photo_200" },
        };
        var queryString = string.Join("&", requestParams
            .Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
        return queryString;
    }
}