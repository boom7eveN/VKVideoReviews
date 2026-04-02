using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class UserRepository(VkVideoReviewsDbContext context) : IUsersRepository
{
    public async Task<UserEntity?> GetUserByVkIdAsync(long vkUserId)
    {
        return await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.VkUserId == vkUserId);
    }

    public async Task<UserEntity?> CreateUserAsync(UserEntity entity)
    {
        await context.Users.AddAsync(entity);
        return entity;
    }

    public void UpdateUserAsync(UserEntity entity)
    {
        context.Users.Update(entity);
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid userId)
    {
        return await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }
}