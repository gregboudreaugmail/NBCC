using Microsoft.AspNetCore.Http;
using NBCC.WebApplication.Messages;
using System.Data.SqlClient;

namespace NBCC.WebApplication;

public class CustomMiddleware
{
    RequestDelegate Next { get; }
    IErrorMessage? ErrorMessage { get; }

    public CustomMiddleware(RequestDelegate next) => Next = next;
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
            await SqlExceptionHandler.LogSqlExceptionAsync(ex);
            await BadRequestResponse.HandleExceptionAsync(httpContext, ErrorMessage?.PersistenceError ?? string.Empty);
        }
        catch (Exception)
        {
            await BadRequestResponse.HandleExceptionAsync(httpContext, ErrorMessage?.GeneralError ?? string.Empty);
        }
    }
}

