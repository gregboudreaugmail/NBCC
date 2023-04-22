using Microsoft.AspNetCore.Http;
using NBCC.WebApplication.Messages;
using System.Data.SqlClient;

namespace NBCC.WebApplicaion;

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
            await SQLExceptionHandler.LogSQLExceptionAsync(ex);
            await BadRequestResponse.HandleExceptionAsync(httpContext, ErrorMessage?.PersistanceError ?? string.Empty);
        }
        catch (Exception)
        {
            await BadRequestResponse.HandleExceptionAsync(httpContext, ErrorMessage?.GeneralError ?? string.Empty);
        }
    }
}

