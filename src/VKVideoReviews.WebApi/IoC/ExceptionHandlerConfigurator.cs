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

                string errorCode, message;
                object? details = null;
                switch (exception)
                {
                    case ModelValidationException validationException:
                        context.Response.StatusCode = validationException.StatusCode;
                        errorCode = validationException.ErrorCode;
                        message = validationException.Message;
                        details = validationException.Errors;
                        break;
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
                    case UnauthorizedAccessException unauthorizedException:
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        errorCode = "UNAUTHORIZED";
                        message = unauthorizedException.Message ?? "User is not authorized";
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
                    var response = new
                    {
                        Code = errorCode, 
                        Message = message, 
                        Errors = details, 
                        Detail = exception.ToString()
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
                else
                {
                    var response = new
                    {
                        Code = errorCode, 
                        Message = message, 
                        Errors = details,
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }));
    }
}