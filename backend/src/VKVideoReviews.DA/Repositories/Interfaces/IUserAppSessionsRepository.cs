using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IUserAppSessionsRepository
{
    Task CreateUserSessionAsync(UserAppSessionEntity sessionEntity);
    Task<UserAppSessionEntity?> GetUserSessionByRefreshTokenHashAsync(string refreshTokenHash);
    void DeleteUserSession(UserAppSessionEntity sessionEntity);
    Task DeleteAllUserSessionsForUserAsync(Guid userId);
}