using NBCC.Authorization.Models;

namespace NBCC.Authorization.DataAccess;

public interface IAuthenticationRepository
{
    Task<User?> Get(string userName);
    Task<bool> AuthenticateUser(string userName, string password);
}
