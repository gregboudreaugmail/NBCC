using NBCC.Middleware.Messages;

namespace NBCC.Middleware;

public class CustomMiddleware
{
    RequestDelegate Next { get; }
    IErrorMessage? ErrorMessage { get; }
    ILoggerAsync? Logger { get; }

    public CustomMiddleware(RequestDelegate next) => Next = next;
    public CustomMiddleware(RequestDelegate next, IErrorMessage errorMessage, ILoggerAsync logger)
    {
        Next = next;
        ErrorMessage = errorMessage;
        Logger = logger;
    }
    public CustomMiddleware(RequestDelegate next, ILoggerAsync logger)
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
            int? messageId = null;
            if (Logger != null)
                messageId = await SqlExceptionHandler.LogSqlException(ex, Logger);
            
            await BadRequestResponse.HandleExceptionAsync(httpContext, 
                ErrorMessage?.PersistenceError ?? string.Empty, messageId);
        }
        catch (Exception ex)
        {
            int? messageId = null;
            if (Logger != null) 
                await ExceptionHandler.LogException(ex, Logger);

            await BadRequestResponse.HandleExceptionAsync(httpContext, 
                ErrorMessage?.GeneralError ?? string.Empty, messageId);
        }
    }
}

