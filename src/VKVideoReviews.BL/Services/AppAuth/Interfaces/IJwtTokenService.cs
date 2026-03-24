using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.BL.Services.AppAuth.Interfaces;

public interface IJwtTokenService
{
    string CreateAccessToken(UserEntity user);
    int GetAccessTokenLifetimeSeconds();
}