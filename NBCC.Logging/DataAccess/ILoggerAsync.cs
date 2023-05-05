using Microsoft.Extensions.Logging;
using NBCC.Logging.Models;

public interface ILoggerAsync
{
    bool IsEnabled(LogLevel logLevel);
    Task Log(Interaction interaction);
    Task Log(LogLevel logLevel, Interaction interaction);
    Task<int?> Log(Exception exception);
    Task<int?> Log(LogLevel logLevel, Exception exception);
}