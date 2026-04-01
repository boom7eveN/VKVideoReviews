using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class UserAppSessionsRepository(VkVideoReviewsDbContext context) : IUserAppSessionsRepository
{
    public async Task AddUserSessionAsync(UserAppSessionEntity sessionEntity)
    {
        await context.UserAppSessions.AddAsync(sessionEntity);
    }

    public async Task<UserAppSessionEntity?> GetUserSessionByRefreshTokenHashAsync(string refreshTokenHash)
    {
        return await context.UserAppSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.AppRefreshTokenHash == refreshTokenHash);
    }

    public void RemoveUserSession(UserAppSessionEntity sessionEntity)
    {
        context.UserAppSessions.Remove(sessionEntity);
    }

    public async Task RemoveAllUserSessionsForUserAsync(Guid userId)
    {
        await context.UserAppSessions.Where(s => s.UserId == userId)
            .ExecuteDeleteAsync();
    }
}