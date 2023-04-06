using Microsoft.AspNetCore.Builder;

namespace NBCC.WebApplicaion;

public static class CustomMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder) 
        => builder.UseMiddleware<CustomMiddleware>();
}