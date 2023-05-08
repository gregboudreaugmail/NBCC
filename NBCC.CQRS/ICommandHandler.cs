namespace NBCC.CQRS.Commands;

public interface ICommandHandler<in TCommand, TCommandResult>
{
    Task<TCommandResult> Handle(TCommand command);
}
public interface ICommandHandler<in TCommand>
{
    Task Handle(TCommand command);
}
