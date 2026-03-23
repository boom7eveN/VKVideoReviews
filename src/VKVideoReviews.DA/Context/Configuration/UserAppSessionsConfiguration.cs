using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Context.Configuration;

public static class UserAppSessionsConfiguration
{
    public static void ConfigureUserAppSessions(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAppSessionEntity>(entity =>
        {
            entity.HasKey(s => s.SessionId);

            entity.Property(s => s.RefreshTokenHash)
                .IsRequired();

            entity.Property(s => s.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(s => s.RefreshTokenHash)
                .HasDatabaseName("IX_UserAppSessions_RefreshTokenHash");

            entity.HasIndex(s => s.UserId)
                .HasDatabaseName("IX_UserAppSessions_UserId");

            entity.HasOne(s => s.User)
                .WithMany(u => u.AppSessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}