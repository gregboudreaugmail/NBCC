namespace NBCC.Authorizaion.DataAccess;

public interface IAuthenticationRepository
{
    Task<bool> ValidateCredentials(string username, string password);
}
