using Microsoft.AspNetCore.Diagnostics;
using TbcTaskApi.Shared.Exceptions;

namespace TbcTaskApi.Middlewares;

public static class CustomExceptionMiddleware
{
    public static void UseCustomExceptionLoggerMiddleware(this IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
    {
        app.UseExceptionHandler(c => c.Run(async context =>
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            var path = context.Features.Get<IExceptionHandlerPathFeature>()?.Path;
            int statusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errors = new List<string>
            {
                exception?.Message
            };

            if (exception?.InnerException != null && env.IsDevelopment())
            {
                errors.Add(exception.InnerException.Message);
            }
            
            logger.LogError(exception.Message, path, statusCode);

            var errorResponse = new ErrorModel
            {
                Path = path,
                Errors = errors,
                HttpStatusCode = statusCode
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }));
    }
}