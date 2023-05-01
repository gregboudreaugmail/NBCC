namespace NBCC.Logging.DataAccess;

public interface IAuthenticationLog 
{
    Task<int> Log(int userId);
}