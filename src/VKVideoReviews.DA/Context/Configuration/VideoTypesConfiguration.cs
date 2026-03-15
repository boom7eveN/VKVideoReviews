using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Context.Configuration;

public static class VideoTypesConfiguration
{
    public static void ConfigureVideoTypes(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VideoTypeEntity>(entity =>
        {
            entity.HasKey(vt => vt.VideoTypeId);
            
            entity.Property(vt => vt.Title)
                .IsRequired()
                .HasMaxLength(20);
            
            entity.HasIndex(vt => vt.Title)
                .IsUnique();
        });
    }
}