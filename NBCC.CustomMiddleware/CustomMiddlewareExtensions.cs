using NBCC.Middleware.Messages;

namespace NBCC.Middleware;

public static class CustomMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder) 
        => builder.UseMiddleware<CustomMiddleware>();

    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder, ErrorMessage errorMessage)
       => builder.UseMiddleware<CustomMiddleware>(errorMessage);
}