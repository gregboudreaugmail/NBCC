namespace NBCC.WebApplication.Messages
{
    public interface IErrorMessage
    {
        string PersistanceError { get; init; }
        string GeneralError { get; init; }
    }
}
