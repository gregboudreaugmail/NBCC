using Microsoft.AspNetCore.Http;

namespace NBCC.Authorizaion;

public class AuthenticatedUser : IAuthenticatedUser
{
    IHttpContextAccessor HttpContextAccessor { get; }
    public string UserName => HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

    public AuthenticatedUser(IHttpContextAccessor httpContextAccessor) => HttpContextAccessor = httpContextAccessor;
}
