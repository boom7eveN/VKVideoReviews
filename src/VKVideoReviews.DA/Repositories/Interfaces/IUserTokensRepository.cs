using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Repositories.Interfaces;

public interface IUserTokensRepository
{
    Task UpsertForUserAsync(UserTokenEntity entity);
}