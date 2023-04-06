namespace NBCC.Authorization;

public interface IUserService
{
    bool ValidateCredentials(string username, string password);
}
