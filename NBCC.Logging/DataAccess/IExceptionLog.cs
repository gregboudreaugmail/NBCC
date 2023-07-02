namespace NBCC.Logging.DataAccess;

public interface IExceptionLog
{
    Task<int> Log(Exception exception);
}