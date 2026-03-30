using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IUserAppSessionsRepository
{
    Task AddAsync(UserAppSessionEntity entity);
    Task<UserAppSessionEntity?> GetByRefreshTokenHashAsync(string refreshTokenHash);
    void Remove(UserAppSessionEntity entity);
    Task RemoveAllForUserAsync(Guid userId);
}