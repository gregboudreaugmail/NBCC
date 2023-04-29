using Microsoft.Extensions.Logging;
using NBCC.Logging.DataAccess;

namespace NBCC.Logging
{
    public class CustomLogger : ILogger
    {
        IInteractionLog InteractionLog { get; }

        public CustomLogger(IInteractionLog interactionLog) => InteractionLog = interactionLog;
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            if (state is not IEnumerable<KeyValuePair<string, object>> logState) return;
            foreach (var ls in logState)
                if (ls.Value is Interaction interaction)
                    InteractionLog.Log(interaction);
        }
    }
}