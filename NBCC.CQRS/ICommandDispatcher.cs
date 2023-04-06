namespace NBCC.Courses.Commands;

public interface ICommandDispatcher
{
    Task Dispatch<TCommand>(TCommand command);
}

public interface ICommandDispatcher<TResult>
{
    Task<TResult> Dispatch<TCommand>(TCommand command);
}
