using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Context.Configuration;

public static class GenresVideosConfiguration
{
    public static void ConfigureGenresVideos(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GenresVideosEntity>(entity =>
        {
            entity.HasKey(gv => new { gv.GenreId, gv.VideoId });
            
            entity.HasOne(gv => gv.Genre)
                .WithMany(g => g.GenresVideos)
                .HasForeignKey(gv => gv.GenreId)
                .OnDelete(DeleteBehavior.Cascade); 
            
            entity.HasOne(gv => gv.Video)
                .WithMany(v => v.GenresVideos)
                .HasForeignKey(gv => gv.VideoId)
                .OnDelete(DeleteBehavior.Cascade); 
        });
    }
}