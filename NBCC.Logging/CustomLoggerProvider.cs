using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using NBCC.Logging.DataAccess;

namespace NBCC.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    IInteractionLog Log { get; }
    private ConcurrentDictionary<string, CustomLogger> Loggers { get; } = new();

    public CustomLoggerProvider(IInteractionLog log) => Log = log;

    public ILogger CreateLogger(string categoryName) => Loggers.GetOrAdd(categoryName, new CustomLogger(Log));

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