using Microsoft.Extensions.Logging;

namespace NBCC.WebApplication;

public static class ExceptionHandler
{
    public static void LogException(Exception ex, ILogger<CustomMiddleware> logger) => 
        logger.LogInformation("{error}", ex.ToString());
}