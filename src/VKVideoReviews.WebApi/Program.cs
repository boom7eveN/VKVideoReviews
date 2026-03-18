using VKVideoReviews.WebApi.IoC;
using VKVideoReviews.WebApi.Settings;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();


var settings = AppSettingsReader.Read(configuration);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Configuration.AddConfiguration(configuration);

SerilogConfigurator.ConfigureServices(builder);
SwaggerConfigurator.ConfigureServices(builder.Services);
MapperConfigurator.ConfigureServices(builder.Services);
ServicesConfigurator.ConfigureServices(builder.Services);
DbContextConfigurator.ConfigureService(builder.Services, settings);

var app = builder.Build();
SerilogConfigurator.ConfigureApplication(app);
ExceptionHandlerConfigurator.ConfigureApplication(app);
SwaggerConfigurator.ConfigureApplication(app);
DbContextConfigurator.ConfigureApplication(app);

app.MapControllers();
app.UseHttpsRedirection();


app.Run();