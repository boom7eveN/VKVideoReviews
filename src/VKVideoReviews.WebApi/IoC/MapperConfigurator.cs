using VKVideoReviews.BL.Mapper;
using VKVideoReviews.WebApi.Mapper;

namespace VKVideoReviews.WebApi.IoC;

public static class MapperConfigurator
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile<GenresWebApiProfile>();
            config.AddProfile<GenresBLProfile>();

            config.AddProfile<VideoTypesBLProfile>();
            config.AddProfile<VideoTypesWebApiProfile>();

            config.AddProfile<VideosWebApiProfile>();
            config.AddProfile<VideosBLProfile>();

            config.AddProfile<VkAuthWebApiProfile>();

            config.AddProfile<AppAuthWebApiProfile>();
            config.AddProfile<AppAuthBLProfile>();

            config.AddProfile<ReviewsWebApiProfile>();
            config.AddProfile<ReviewsBLProfile>();
            
            config.AddProfile<FavoriteBLProfile>();
            config.AddProfile<FavoriteWebApiProfile>();
        });
    }
}