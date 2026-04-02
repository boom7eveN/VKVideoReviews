using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.DA.Entities;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.DA.Repositories;

public class UserTokensRepository(VkVideoReviewsDbContext context) : IUserTokensRepository
{
    public async Task UpsertTokensForUserAsync(UserTokenEntity entity)
    {
        await context.Database.ExecuteSqlInterpolatedAsync(
            $@"
            INSERT INTO ""UserTokens"" (
                ""TokenRecordId"", 
                ""UserId"", 
                ""VkUserId"", 
                ""VkAccessTokenEncrypted"", 
                ""VkRefreshTokenEncrypted"",
                ""AccessTokenExpiresAt"", 
                ""RefreshTokenExpiresAt"", 
                ""CreatedAt""
            )
            VALUES (
                {entity.TokenRecordId}, 
                {entity.UserId}, 
                {entity.VkUserId}, 
                {entity.VkAccessTokenEncrypted}, 
                {entity.VkRefreshTokenEncrypted},
                {entity.AccessTokenExpiresAt}, 
                {entity.RefreshTokenExpiresAt}, 
                {entity.CreatedAt}
            )
            ON CONFLICT (""UserId"") DO UPDATE SET
                ""VkUserId"" = EXCLUDED.""VkUserId"",
                ""VkAccessTokenEncrypted"" = EXCLUDED.""VkAccessTokenEncrypted"",
                ""VkRefreshTokenEncrypted"" = EXCLUDED.""VkRefreshTokenEncrypted"",
                ""AccessTokenExpiresAt"" = EXCLUDED.""AccessTokenExpiresAt"",
                ""RefreshTokenExpiresAt"" = EXCLUDED.""RefreshTokenExpiresAt"",
                ""CreatedAt"" = EXCLUDED.""CreatedAt"";
            ");
    }
}