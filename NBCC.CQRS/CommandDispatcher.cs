using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NBCC.Logging.Models;
using static System.Reflection.Assembly;
using static System.Text.Json.JsonSerializer;

namespace NBCC.Courses.Commands;

public sealed class CommandDispatcher : ICommandDispatcher
{
    IServiceProvider ServiceProvider { get; }
    ILogger<CommandDispatcher> Logger { get; }

    public CommandDispatcher(IServiceProvider serviceProvider, ILogger<CommandDispatcher> logger)
    {
        ServiceProvider = serviceProvider;
        Logger = logger;
    }

    public async Task Dispatch<TCommand>(TCommand command)
    {
        var handler = ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        LogInteraction(command, handler);
        await handler.Handle(command);
    }

    private void LogInteraction<TCommand>(TCommand command, ICommandHandler<TCommand> handler)
    {
        try
        {
            var assembly = GetEntryAssembly()?.ManifestModule.Name ?? string.Empty;
            var commandParameters = Serialize(command);
            var commandName = handler.GetType().FullName ?? string.Empty;
            Logger.LogInformation("{log}", new Interaction(assembly, commandName, commandParameters));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
