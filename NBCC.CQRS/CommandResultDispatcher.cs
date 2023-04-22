using Microsoft.Extensions.DependencyInjection;

namespace NBCC.Courses.Commands;

public sealed class CommandResultDispatcher<TResult> : ICommandDispatcher<TResult>
{
    private readonly IServiceProvider _serviceProvider;

    public CommandResultDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task<TResult> Dispatch<TCommand>(TCommand command)
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.Handle(command);
    }
}
