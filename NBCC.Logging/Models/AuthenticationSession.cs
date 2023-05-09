namespace NBCC.Logging.Models;

public sealed class AuthenticationSession : IAuthenticationSession
{
    IHttpContextAccessor HttpContextAccessor { get; }

    public AuthenticationSession(IHttpContextAccessor httpContextAccessorAccessor) => HttpContextAccessor = httpContextAccessorAccessor;
    public int? AuthenticationId
    {
        get
        {
            var authenticateId = HttpContextAccessor.HttpContext.Request.Headers[CustomHeaders.AuthenticatedId].FirstOrDefault();
            return authenticateId == null ? null : int.Parse(authenticateId);
        }
    }

    public void AssignAuthentication(int authenticationId) => 
        HttpContextAccessor.HttpContext.Request.Headers.Add(CustomHeaders.AuthenticatedId, authenticationId.ToString());
}