using NBCC.Logging.Models;
namespace NBCC.Courses.Commands;

public sealed class CommandDispatcher : ICommandDispatcher
{
    IServiceProvider ServiceProvider { get; }
    ILoggerAsync Logger { get; }

    public CommandDispatcher(IServiceProvider serviceProvider, ILoggerAsync logger)
    {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Dispatch<TCommand>(TCommand command)
    {
        var handler = ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        LogInteraction(command, handler);
        await handler.Handle(command);
    }

    private void LogInteraction<TCommand>(TCommand command, ICommandHandler<TCommand> handler)
    {
        var assembly = GetEntryAssembly()?.ManifestModule.Name ?? string.Empty;
        var commandParameters = Serialize(command);
        var commandName = handler.GetType().FullName ?? string.Empty;
        Logger.Log(new Interaction(assembly, commandName, commandParameters));
    }
}
