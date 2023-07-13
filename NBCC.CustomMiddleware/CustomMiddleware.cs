using NBCC.Middleware.Messages;

namespace NBCC.Middleware;

public class CustomMiddleware
{
    RequestDelegate Next { get; }
    ErrorMessage? ErrorMessage { get; }
    ILoggerAsync? Logger { get; }

    public CustomMiddleware(RequestDelegate next) => Next = next;
    public CustomMiddleware(RequestDelegate next, ErrorMessage errorMessage, ILoggerAsync logger)
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

    public CustomMiddleware(RequestDelegate next, ErrorMessage errorMessage)
    {
        Next = next;
        ErrorMessage = errorMessage;
    }

    /*
     * Note 30
     * Exceptions
     * Finally, we're going to cover exception logging.  You may have noticed that there
     * aren't any try/catch blocks in my code.  That's because any exception will spill
     * over here in the custom middleware where I can handle it appropriately.  For SQL
     * related errors, I can add a little more detail then a general exception for example.
     */
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

