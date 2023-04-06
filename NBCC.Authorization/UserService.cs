namespace NBCC.Authorization;

public sealed class UserService : IUserService
{
    public bool ValidateCredentials(string username, string password)
    {
        return username.Equals("a") && password.Equals("a");
    }
}