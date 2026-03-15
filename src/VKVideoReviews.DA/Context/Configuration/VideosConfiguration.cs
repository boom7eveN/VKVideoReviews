using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Context.Configuration;

public static class VideosConfiguration
{
    public static void ConfigureVideos(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VideoEntity>(entity =>
        {
            entity.HasKey(v => v.VideoId);
            
            entity.HasIndex(v => v.VkVideoId);
            entity.Property(v => v.AverageRate)
                .HasDefaultValue(0.0);
            
            entity.Property(v => v.TotalReviews)
                .HasDefaultValue(0);
            entity.HasOne(v => v.VideoType)
                .WithMany(vt => vt.Videos)
                .HasForeignKey(v => v.VideoTypeId)
                .OnDelete(DeleteBehavior.Restrict); 
        });
    }
}