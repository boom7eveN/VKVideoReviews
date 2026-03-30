using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class UserAppSessionsRepository(VkVideoReviewsDbContext context) : IUserAppSessionsRepository
{
    public async Task AddAsync(UserAppSessionEntity entity)
    {
        await context.UserAppSessions.AddAsync(entity);
    }

    public async Task<UserAppSessionEntity?> GetByRefreshTokenHashAsync(string refreshTokenHash)
    {
        return await context.UserAppSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.RefreshTokenHash == refreshTokenHash);
    }

    public void Remove(UserAppSessionEntity entity)
    {
        context.UserAppSessions.Remove(entity);
    }

    public async Task RemoveAllForUserAsync(Guid userId)
    {
        await context.UserAppSessions.Where(s => s.UserId == userId)
            .ExecuteDeleteAsync();
    }
}