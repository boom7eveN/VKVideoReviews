using Microsoft.Extensions.Caching.Memory;
using VKVideoReviews.BL.Services.Genres;
using VKVideoReviews.BL.Services.VideoTypes;
using VKVideoReviews.BL.Services.VkAuth;
using VKVideoReviews.DA.Repositories;
using VKVideoReviews.DA.Repositories.Interfaces;
using VKVideoReviews.WebApi.Settings;

namespace VKVideoReviews.WebApi.IoC;

public static class ServicesConfigurator
{
    public static void ConfigureServices(IServiceCollection services, AppSettings appSettings)
    {
        services.AddScoped<IGenresService, GenresService>();
        services.AddScoped<IVideoTypesService, VideoTypesService>();


        services.AddScoped<IGenresRepository, GenresRepository>();
        services.AddScoped<IVideoTypesRepository, VideoTypesRepository>();

        services.AddHttpClient();
        services.AddScoped<IVkAuthService>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            var cache = sp.GetRequiredService<IMemoryCache>();

            return new VkAuthService(
                appSettings.ClientId,
                appSettings.RedirectUri,
                httpClient,
                cache
                );
        });
    }
}