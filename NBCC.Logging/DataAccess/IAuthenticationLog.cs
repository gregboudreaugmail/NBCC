using NBCC.Logging.Models;

namespace NBCC.Logging.DataAccess;

public interface IAuthenticationLog 
{
    Task<int> Log(Authentication authentication);
}