using Microsoft.Extensions.DependencyInjection;

namespace NBCC.Courses.Commands;

public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task Dispatch<TCommand>(TCommand command)
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.Handle(command);
    }
}
