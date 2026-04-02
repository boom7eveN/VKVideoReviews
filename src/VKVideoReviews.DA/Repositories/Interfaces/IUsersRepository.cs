using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IUsersRepository
{
    Task<UserEntity?> GetUserByVkIdAsync(long vkUserId);
    Task<UserEntity?> CreateUserAsync(UserEntity user);
    void UpdateUserAsync(UserEntity user);
    Task<UserEntity?> GetUserByIdAsync(Guid userId);
}