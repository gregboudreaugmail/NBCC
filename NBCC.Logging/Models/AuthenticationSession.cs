using Microsoft.AspNetCore.Http;

namespace NBCC.Logging.Models;

public sealed class AuthenticationSession : IAuthenticationSession
{
    IHttpContextAccessor HttpContextAccessor { get; }

    public AuthenticationSession(IHttpContextAccessor httpContextAccessorAccessor) => HttpContextAccessor = httpContextAccessorAccessor;
    public int UserId => int.Parse(HttpContextAccessor.HttpContext.Request.Headers[CachedItems.UserId].FirstOrDefault() ?? "0");
    public int AuthenticationId =>  int.Parse(HttpContextAccessor.HttpContext.Request.Headers[CachedItems.AuthenticatedId].FirstOrDefault()  ?? "0");

    public void AssignAuthentication(int authenticationId, int userId)
    {
        HttpContextAccessor.HttpContext.Request.Headers.Add(CachedItems.UserId, userId.ToString());
        HttpContextAccessor.HttpContext.Request.Headers.Add(CachedItems.AuthenticatedId, authenticationId.ToString());
    }
}