using VKVideoReviews.WebApi.Settings;

namespace VKVideoReviews.WebApi.IoC;

public static class CorsConfigurator
{
    public const string PolicyName = "SpaCorsPolicy";

    public static void ConfigureServices(IServiceCollection services, AppSettings settings)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                var allowedOrigins = settings.CorsAllowedOrigins;

                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public static void ConfigureApplication(IApplicationBuilder app)
    {
        app.UseCors(PolicyName);
    }
}
