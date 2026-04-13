using Microsoft.EntityFrameworkCore;
using VKVideoReviews.DA.Context;
using VKVideoReviews.WebApi.Settings;

namespace VKVideoReviews.WebApi.IoC;

public static class DbContextConfigurator
{
    public static void ConfigureService(IServiceCollection services, AppSettings settings)
    {
        services.AddDbContextFactory<VkVideoReviewsDbContext>(
            options => { options.UseNpgsql(settings.VkVideoReviewsDbConnectionString); }, ServiceLifetime.Scoped);
    }

    public static void ConfigureApplication(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var contextFactory =
            scope.ServiceProvider.GetRequiredService<IDbContextFactory<VkVideoReviewsDbContext>>();
        using var context = contextFactory.CreateDbContext();
        context.Database.Migrate();
    }
}