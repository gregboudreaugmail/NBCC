using NBCC.Authorization.Models;

namespace NBCC.Authorization.DataAccess;

public interface IAuthenticationRepository
{
    Task<User?> GetUser(string userName);
    Task<bool> AuthenticateUser(string userName, string password);
}
