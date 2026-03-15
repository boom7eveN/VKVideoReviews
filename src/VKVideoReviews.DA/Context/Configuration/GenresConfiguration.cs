using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Context.Configuration;

public static class GenresConfiguration
{
    public static void ConfigureGenres(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GenreEntity>(entity =>
        {
            entity.HasKey(g => g.GenreId);

            entity.Property(g => g.Title)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(g => g.Title)
                .IsUnique();
        });
    }
}