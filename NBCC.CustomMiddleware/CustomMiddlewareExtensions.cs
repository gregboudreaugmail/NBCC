﻿using NBCC.WebApplication.Messages;

namespace NBCC.WebApplication;

public static class CustomMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder) 
        => builder.UseMiddleware<CustomMiddleware>();

    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder, IErrorMessage errorMessage)
       => builder.UseMiddleware<CustomMiddleware>(errorMessage);
}