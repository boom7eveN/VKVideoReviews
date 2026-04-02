using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using VKVideoReviews.BL.Clients;
using VKVideoReviews.BL.Clients.Interfaces;
using VKVideoReviews.BL.Services.AppAuth;
using VKVideoReviews.BL.Services.AppAuth.Interfaces;
using VKVideoReviews.BL.Services.AppAuth.Models;
using VKVideoReviews.BL.Services.Favorite;
using VKVideoReviews.BL.Services.Favorite.Interfaces;
using VKVideoReviews.BL.Services.Genres;
using VKVideoReviews.BL.Services.Genres.Interfaces;
using VKVideoReviews.BL.Services.Reviews;
using VKVideoReviews.BL.Services.Reviews.Interfaces;
using VKVideoReviews.BL.Services.Videos;
using VKVideoReviews.BL.Services.Videos.Interfaces;
using VKVideoReviews.BL.Services.VideoTypes;
using VKVideoReviews.BL.Services.VideoTypes.Interfaces;
using VKVideoReviews.BL.Services.VkAuth;
using VKVideoReviews.BL.Services.VkAuth.Interfaces;
using VKVideoReviews.DA.Repositories;
using VKVideoReviews.DA.Repositories.Interfaces;
using VKVideoReviews.DA.UnitOfWork;
using VKVideoReviews.DA.UnitOfWork.Interfaces;
using VKVideoReviews.WebApi.Settings;

namespace VKVideoReviews.WebApi.IoC;

public static class ServicesConfigurator
{
    public static void ConfigureServices(IServiceCollection services, AppSettings appSettings)
    {
        services.AddScoped<IGenresRepository, GenresRepository>();
        services.AddScoped<IVideoTypesRepository, VideoTypesRepository>();
        services.AddScoped<IUserTokensRepository, UserTokensRepository>();
        services.AddScoped<IUsersRepository, UserRepository>();
        services.AddScoped<IUserAppSessionsRepository, UserAppSessionsRepository>();
        services.AddScoped<IVideosRepository, VideosRepository>();
        services.AddScoped<IGenresVideosRepository, GenresVideosRepository>();
        services.AddScoped<IReviewsRepository, ReviewsRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();

        services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IGenresService, GenresService>();
        services.AddScoped<IVideoTypesService, VideoTypesService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IVideosService, VideosService>();
        services.AddScoped<IReviewsService, ReviewsService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddSingleton<ITokenEncryptionService>(_ =>
            new AesGcmTokenEncryptionService(appSettings.EncryptionKey));


        services.AddSingleton(appSettings.JwtAuthSettings);

        services.AddHttpClient<IVkApiAuthClient, VkApiAuthClient>(client =>
        {
            client.BaseAddress = new Uri(appSettings.VkIdUri);
        });
        services.AddScoped<IVkAuthService>(sp =>
        {
            var vkIdClient = sp.GetRequiredService<IVkApiAuthClient>();
            var cache = sp.GetRequiredService<IMemoryCache>();

            return new VkAuthService(
                appSettings.ClientId,
                appSettings.RedirectUri,
                vkIdClient,
                cache
            );
        });

        services.AddHttpClient<IVkApiMethodsClient, VkApiMethodsClient>(client =>
        {
            client.BaseAddress = new Uri(appSettings.VkMethodsUri);
        });
        services.AddScoped<IAppAuthService>(sp =>
        {
            var vkMethodsClient = sp.GetRequiredService<IVkApiMethodsClient>();
            var mapper = sp.GetRequiredService<IMapper>();
            var unitOfWork = sp.GetRequiredService<IAuthUnitOfWork>();
            var jwtAuthSettings = sp.GetRequiredService<JwtAuthSettings>();
            var jwtTokenService = sp.GetRequiredService<IJwtTokenService>();
            var tokenEncryptionService = sp.GetRequiredService<ITokenEncryptionService>();
            return new AppAuthService(
                vkMethodsClient,
                mapper,
                unitOfWork,
                jwtAuthSettings,
                jwtTokenService,
                tokenEncryptionService,
                appSettings.AdminVkUserIds);
        });
    }
}