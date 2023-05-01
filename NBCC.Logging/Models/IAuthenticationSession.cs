namespace NBCC.Logging.Models;

public interface IAuthenticationSession
{
    int UserId { get; }
    int AuthenticationId { get; }
    void AssignAuthentication(int authenticationId, int userId);
}