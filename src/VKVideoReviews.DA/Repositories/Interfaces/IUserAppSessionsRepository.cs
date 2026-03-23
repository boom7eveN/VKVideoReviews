using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IUserAppSessionsRepository
{
    Task AddAsync(UserAppSessionEntity entity);
    Task<UserAppSessionEntity?> GetByRefreshTokenHashAsync(string refreshTokenHash);
    Task RemoveAsync(UserAppSessionEntity entity);
    Task RemoveAllForUserAsync(Guid userId);
}