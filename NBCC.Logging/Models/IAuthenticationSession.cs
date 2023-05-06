namespace NBCC.Logging.Models;

public interface IAuthenticationSession
{
    int AuthenticationId { get; }
    void AssignAuthentication(int authenticationId);
}