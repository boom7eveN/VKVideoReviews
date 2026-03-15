using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Context.Configuration;

public static class UsersConfiguration
{
    public static void ConfigureUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.IsAdmin)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasIndex(u => u.VkUserId)
                .IsUnique();
        });
    }
}