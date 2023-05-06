using NBCC.WebApplication.Messages;

namespace NBCC.Courses.WebApplication.Messages;

public sealed record Message : IErrorMessage
{
    public string PersistenceError { get; init; } = string.Empty;
    public string GeneralError { get; init; } = string.Empty;
}
