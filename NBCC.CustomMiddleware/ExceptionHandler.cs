namespace NBCC.WebApplication;

public static class ExceptionHandler
{
    public static async Task<int?> LogException(Exception ex, ILoggerAsync logger) => 
        await logger.Log(ex);
}