using Microsoft.AspNetCore.Http;

namespace NBCC.Logging.Models;

public sealed class AuthenticationSession : IAuthenticationSession
{
    IHttpContextAccessor HttpContextAccessor { get; }

    public AuthenticationSession(IHttpContextAccessor httpContextAccessorAccessor) => HttpContextAccessor = httpContextAccessorAccessor;
    public int AuthenticationId =>  int.Parse(HttpContextAccessor.HttpContext.Request.Headers[CustomHeaders.AuthenticatedId].FirstOrDefault()  ?? "0");

    public void AssignAuthentication(int authenticationId) => 
        HttpContextAccessor.HttpContext.Request.Headers.Add(CustomHeaders.AuthenticatedId, authenticationId.ToString());
}