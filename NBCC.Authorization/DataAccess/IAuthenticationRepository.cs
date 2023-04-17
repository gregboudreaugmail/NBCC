namespace NBCC.Authorizaion.DataAccess;

public interface IAuthenticationRepository
{
    Task<User?> GetUser(string userName);
    Task<bool> AuthenticateUser(string userName, string password);
}
