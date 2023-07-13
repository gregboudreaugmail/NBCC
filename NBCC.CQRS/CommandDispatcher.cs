using NBCC.Logging.Models;
namespace NBCC.CQRS.Commands;

/*
 * Note 29
 * Auditing each call
 * Another benefit of having an in house CQRS system is that it makes auditing very easy.
 * Without needing the developer to copy and paste logging code on every function they make,
 * we can make the dispatcher log.
 *
 */
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


public sealed class CommandDispatcher<T> : ICommandDispatcher<T>
{
    IServiceProvider ServiceProvider { get; }
    ILoggerAsync Logger { get; }

    public CommandDispatcher(IServiceProvider serviceProvider, ILoggerAsync logger)
    {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<T> Dispatch<TCommand>(TCommand command)
    {
        var handler = ServiceProvider.GetRequiredService<ICommandHandler<TCommand, T>>();
        LogInteraction(command, handler);
        return await handler.Handle(command);
    }

    private void LogInteraction<TCommand>(TCommand command, ICommandHandler<TCommand, T> handler)
    {
        var assembly = GetEntryAssembly()?.ManifestModule.Name ?? string.Empty;
        var commandParameters = Serialize(command);
        var commandName = handler.GetType().FullName ?? string.Empty;
        Logger.Log(new Interaction(assembly, commandName, commandParameters));
    }
}
