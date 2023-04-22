namespace NBCC.WebApplication.Messages
{
    public interface IErrorMessage
    {
        string PersistenceError { get; init; }
        string GeneralError { get; init; }
    }
}
