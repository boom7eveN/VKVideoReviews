using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Context.Configuration;

public static class UserTokensConfiguration
{
    public static void ConfigureUserTokens(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserTokenEntity>(entity =>
        {
            entity.HasKey(t => t.TokenRecordId);
            
            entity.HasIndex(e => e.UserId)
                .IsUnique();

            entity.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(t => t.VkUserId)
                .HasDatabaseName("IX_UserTokens_VkUserId");

            entity.HasIndex(t => t.AccessTokenExpiresAt)
                .HasDatabaseName("IX_UserTokens_ExpiresAt");

            entity.HasIndex(t => t.RefreshToken)
                .HasDatabaseName("IX_UserTokens_RefreshToken");

            entity.HasOne(t => t.User)
                .WithMany(u => u.Tokens)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}