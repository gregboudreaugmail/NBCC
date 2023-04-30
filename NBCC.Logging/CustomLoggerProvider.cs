using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using NBCC.Logging.DataAccess;

namespace NBCC.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    IInteractionLog InteractionLog { get; }
    IAuthenticationLog AuthenticationLog { get; }
    private ConcurrentDictionary<string, CustomLogger> Loggers { get; } = new();

    public CustomLoggerProvider(IInteractionLog interactionLog , IAuthenticationLog authenticationLog)
    {
        InteractionLog = interactionLog;
        AuthenticationLog = authenticationLog;
    }

    public ILogger CreateLogger(string categoryName) => Loggers.GetOrAdd(categoryName, new CustomLogger(InteractionLog, AuthenticationLog));

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (disposing) Loggers.Clear();
    }
}