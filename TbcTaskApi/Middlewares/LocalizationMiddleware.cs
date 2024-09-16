using System.Globalization;

namespace TbcTaskApi.Middlewares;

public class LocalizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var acceptLanguageHeader = context.Request.Headers["Accept-Language"].ToString();

        if (!string.IsNullOrEmpty(acceptLanguageHeader))
        {
            var culture = new CultureInfo(acceptLanguageHeader.Split(',')[0]);
            Thread.CurrentThread.CurrentCulture = culture;
        }
        
        await next(context);
    }
}
