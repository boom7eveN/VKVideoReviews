using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Context.Configuration;

public static class FavoriteConfiguration
{
    public static void ConfigureFavorite(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavoriteEntity>(entity =>
        {
            entity.HasKey(f => new { f.FavoriteId });

            entity.HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(f => f.Video)
                .WithMany(v => v.Favorites)
                .HasForeignKey(f => f.VideoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}