using Microsoft.Extensions.Logging;
using NBCC.Logging.DataAccess;
using NBCC.Logging.Models;

namespace NBCC.Logging
{
    public class CustomLogger : ILogger
    {
        IInteractionLog InteractionLog { get; }
        IAuthenticationLog AuthenticationLog { get; }

        public CustomLogger(IInteractionLog interactionLog, IAuthenticationLog authenticationLog)
        {
            InteractionLog = interactionLog;
            AuthenticationLog = authenticationLog;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            if (state is not IEnumerable<KeyValuePair<string, object>> logState) return;
            foreach (var ls in logState)
            {
                switch (ls.Value)
                {
                    case Interaction interaction:
                        InteractionLog.Log(interaction).Wait();
                        break;
                    case Authentication authentication:
                        var x = AuthenticationLog.Log(authentication).Result;

                        break;
                }
            }
        }
    }
}