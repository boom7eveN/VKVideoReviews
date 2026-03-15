using VKVideoReviews.WebApi.IoC;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(configuration);

SerilogConfigurator.ConfigureServices(builder);
SwaggerConfigurator.ConfigureServices(builder.Services);


var app = builder.Build();
SerilogConfigurator.ConfigureApplication(app);
SwaggerConfigurator.ConfigureApplication(app);


app.UseHttpsRedirection();


app.Run();