using VKVideoReviews.WebApi.IoC;
using VKVideoReviews.WebApi.Settings;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();


var settings = AppSettingsReader.Read(configuration);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(configuration);

SerilogConfigurator.ConfigureServices(builder);
SwaggerConfigurator.ConfigureServices(builder.Services);
DbContextConfigurator.ConfigureService(builder.Services, settings);

var app = builder.Build();
SerilogConfigurator.ConfigureApplication(app);
SwaggerConfigurator.ConfigureApplication(app);
DbContextConfigurator.ConfigureApplication(app);


app.UseHttpsRedirection();


app.Run();