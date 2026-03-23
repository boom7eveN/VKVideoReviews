using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IUsersRepository
{
    Task<UserEntity?> GetByVkUserIdAsync(long vkUserId);
    Task<UserEntity?> AddAsync(UserEntity user);
    Task UpdateAsync(UserEntity user);
    Task<UserEntity?> GetByIdAsync(Guid userId);
}