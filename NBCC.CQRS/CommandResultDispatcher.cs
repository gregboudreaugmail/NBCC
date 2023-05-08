namespace NBCC.CQRS.Commands;

public sealed class CommandResultDispatcher<TResult> : ICommandDispatcher<TResult>
{
    private IServiceProvider ServiceProvider { get; }

    public CommandResultDispatcher(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;

    public async Task<TResult> Dispatch<TCommand>(TCommand command)
    {
        var handler = ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.Handle(command);
    }
}
