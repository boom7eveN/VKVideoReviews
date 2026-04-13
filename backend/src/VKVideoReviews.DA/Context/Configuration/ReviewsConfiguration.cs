using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Context.Configuration;

public static class ReviewsConfiguration
{
    public static void ConfigureReviews(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>(entity =>
        {
            entity.HasKey(r => new { r.ReviewId });

            entity.HasIndex(r => new { r.UserId, r.VideoId })
                .IsUnique();

            entity.Property(r => r.Rate)
                .IsRequired();

            entity.ToTable(t => t.HasCheckConstraint("CK_Review_Rate", "\"Rate\" >= 1 AND \"Rate\" <= 10"));


            entity.Property(r => r.CreateDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(r => r.UpdateDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Video)
                .WithMany(v => v.Reviews)
                .HasForeignKey(r => r.VideoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(r => r.Rate);
        });
    }
}