using VKVideoReviews.BL.Services.Genres;
using VKVideoReviews.DA.Repositories;
using VKVideoReviews.DA.Repositories.Interfaces;

namespace VKVideoReviews.WebApi.IoC;

public class ServicesConfigurator
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IGenresService, GenresService>();


        services.AddScoped<IGenresRepository, GenresRepository>();
    }
}