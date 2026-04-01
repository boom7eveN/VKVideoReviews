using VKVideoReviews.WebApi.IoC;
using VKVideoReviews.WebApi.Settings;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();


var settings = AppSettingsReader.Read(configuration);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => { options.LowercaseUrls = true; });
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Configuration.AddConfiguration(configuration);

//TODO: Подумать чё делать с репозиториями(отказаться от дженерика/унифицировать как то)
//TODO: ВЕЗДЕ в интерфейсах и имплемент. репозиториев унифицировать названия методов и нормлально прописать названия параметров
//TODO: То же самое что и выше ток в сервисах и контроллерах чтобы по названию параметра было понятно что передавать а не тупо id

SerilogConfigurator.ConfigureServices(builder);
SwaggerConfigurator.ConfigureServices(builder.Services);
MapperConfigurator.ConfigureServices(builder.Services);
ServicesConfigurator.ConfigureServices(builder.Services, settings);
JwtAuthenticationConfigurator.ConfigureServices(builder.Services, settings);
DbContextConfigurator.ConfigureService(builder.Services, settings);

var app = builder.Build();
SerilogConfigurator.ConfigureApplication(app);
ExceptionHandlerConfigurator.ConfigureApplication(app);
SwaggerConfigurator.ConfigureApplication(app);
DbContextConfigurator.ConfigureApplication(app);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();