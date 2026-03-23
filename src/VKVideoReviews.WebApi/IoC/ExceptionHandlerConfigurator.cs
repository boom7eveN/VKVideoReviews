using Microsoft.AspNetCore.Diagnostics;
using VKVideoReviews.BL.Exceptions.BusinessLogicExceptions;
using VKVideoReviews.BL.Exceptions.VkAuthExceptions;

namespace VKVideoReviews.WebApi.IoC;

public static class ExceptionHandlerConfigurator
{
    public static void ConfigureApplication(IApplicationBuilder app)
    {
        app.UseExceptionHandler(config =>
            config.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception is null)
                    return;
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

                logger.LogError(exception,
                    "Unhandled exception: {Message}\nPath: {Path}\nMethod: {Method}",
                    exception.Message,
                    context.Request.Path,
                    context.Request.Method);

                string errorCode, message;
                switch (exception)
                {
                    case BusinessLogicException businessLogicException:
                        context.Response.StatusCode = businessLogicException.StatusCode;
                        errorCode = businessLogicException.ErrorCode;
                        message = businessLogicException.Message;
                        break;
                    case VkAuthException authException:
                        context.Response.StatusCode = authException.StatusCode;
                        errorCode = authException.ErrorCode;
                        message = authException.Message;
                        break;
                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        errorCode = "INTERNAL_ERROR";
                        message = "Internal Server Error";
                        break;
                }

                var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
                if (environment.IsDevelopment())
                {
                    var response = new { Code = errorCode, Message = message, Detail = exception.ToString() };
                    await context.Response.WriteAsJsonAsync(response);
                }
                else
                {
                    var response = new { Code = errorCode, Message = message };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }));
    }
}