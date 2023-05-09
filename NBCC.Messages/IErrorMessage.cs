namespace NBCC.Messages;

public interface IErrorMessage
{
    string PersistenceError { get; init; }
    string GeneralError { get; init; }
}

public interface IAuthenticationMessage
{
    string AuthenticationFailed { get; init; }
}