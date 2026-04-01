using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IUserAppSessionsRepository
{
    Task AddUserSessionAsync(UserAppSessionEntity sessionEntity);
    Task<UserAppSessionEntity?> GetUserSessionByRefreshTokenHashAsync(string refreshTokenHash);
    void RemoveUserSession(UserAppSessionEntity sessionEntity);
    Task RemoveAllUserSessionsForUserAsync(Guid userId);
}