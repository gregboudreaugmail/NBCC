namespace NBCC.Middleware.Messages;

public sealed record ErrorMessage 
{
    public string PersistenceError { get; init; } = string.Empty;
    public string GeneralError { get; init; } = string.Empty;
}