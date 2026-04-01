using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class UserTokensRepository(VkVideoReviewsDbContext context) : IUserTokensRepository
{
    public async Task UpsertForUserAsync(UserTokenEntity entity)
    {
        await context.Database.ExecuteSqlInterpolatedAsync(
            $@"
            INSERT INTO ""UserTokens"" (
                ""TokenRecordId"", 
                ""UserId"", 
                ""VkUserId"", 
                ""VkAccessTokenHash"", 
                ""VkRefreshTokenHash"",
                ""AccessTokenExpiresAt"", 
                ""RefreshTokenExpiresAt"", 
                ""CreatedAt""
            )
            VALUES (
                {entity.TokenRecordId}, 
                {entity.UserId}, 
                {entity.VkUserId}, 
                {entity.VkAccessTokenHash}, 
                {entity.VkRefreshTokenHash},
                {entity.AccessTokenExpiresAt}, 
                {entity.RefreshTokenExpiresAt}, 
                {entity.CreatedAt}
            )
            ON CONFLICT (""UserId"") DO UPDATE SET
                ""VkUserId"" = EXCLUDED.""VkUserId"",
                ""VkAccessTokenHash"" = EXCLUDED.""VkAccessTokenHash"",
                ""VkRefreshTokenHash"" = EXCLUDED.""VkRefreshTokenHash"",
                ""AccessTokenExpiresAt"" = EXCLUDED.""AccessTokenExpiresAt"",
                ""RefreshTokenExpiresAt"" = EXCLUDED.""RefreshTokenExpiresAt"",
                ""CreatedAt"" = EXCLUDED.""CreatedAt"";
            ");
    }
}