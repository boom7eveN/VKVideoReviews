using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context.Configuration;
using VKVideoReviews.DA.Entities;

namespace VKVideoReviews.DA.Context;

public class VkVideoReviewsDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<VideoEntity> Videos { get; set; }
    public DbSet<VideoTypeEntity> VideoTypes { get; set; }
    public DbSet<GenreEntity> Genres { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<GenresVideosEntity> GenresVideosEntities { get; set; }
    public DbSet<FavoriteEntity> Favorite { get; set; }
    public DbSet<UserTokenEntity> UserTokens { get; set; }
    public DbSet<UserAppSessionEntity> UserAppSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureUsers();
        modelBuilder.ConfigureVideos();
        modelBuilder.ConfigureFavorite();
        modelBuilder.ConfigureVideoTypes();
        modelBuilder.ConfigureGenres();
        modelBuilder.ConfigureReviews();
        modelBuilder.ConfigureGenresVideos();
        modelBuilder.ConfigureUserTokens();
        modelBuilder.ConfigureUserAppSessions();
    }
}