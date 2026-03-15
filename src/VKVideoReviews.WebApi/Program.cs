using VKVideoReviews.WebApi.IoC;

var builder = WebApplication.CreateBuilder(args);
SerilogConfigurator.ConfigureServices(builder);
SwaggerConfigurator.ConfigureServices(builder.Services);


var app = builder.Build();
SerilogConfigurator.ConfigureApplication(app);
SwaggerConfigurator.ConfigureApplication(app);


app.UseHttpsRedirection();


app.Run();