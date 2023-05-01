using Microsoft.AspNetCore.Http;
using NBCC.WebApplication.Messages;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace NBCC.WebApplication;

public class CustomMiddleware
{
    RequestDelegate Next { get; }
    IErrorMessage? ErrorMessage { get; }
    ILogger<CustomMiddleware>? Logger { get; }

    public CustomMiddleware(RequestDelegate next) => Next = next;
    public CustomMiddleware(RequestDelegate next, IErrorMessage errorMessage, ILogger<CustomMiddleware> logger)
    {
        Next = next;
        ErrorMessage = errorMessage;
        Logger = logger;
    }
    public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger)
    {
        Next = next;
        Logger = logger;
    }

    public CustomMiddleware(RequestDelegate next, IErrorMessage errorMessage)
    {
        Next = next;
        ErrorMessage = errorMessage;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await Next(httpContext);
        }
        catch (SqlException ex)
        {   
            if (Logger != null) SqlExceptionHandler.LogSqlException(ex, Logger);
            await BadRequestResponse.HandleExceptionAsync(httpContext, ErrorMessage?.PersistenceError ?? string.Empty);
        }
        catch (Exception ex)
        {
            if (Logger != null) ExceptionHandler.LogException(ex, Logger);
            await BadRequestResponse.HandleExceptionAsync(httpContext, ErrorMessage?.GeneralError ?? string.Empty);
        }
    }
}

