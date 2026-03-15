using Microsoft.OpenApi;

namespace VKVideoReviews.WebApi.IoC;

public static class SwaggerConfigurator
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "VKVideoReviews API", Version = "v1" });
        });
    }
    
    public static void ConfigureApplication(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}