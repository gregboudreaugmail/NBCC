using Microsoft.AspNetCore.Http;
using NBCC.Authorization.DataAccess;
using NBCC.Authorization.Models;
using NBCC.Logging.DataAccess;
using NBCC.Logging.Models;

namespace NBCC.Authorization;

public sealed class TicketCreator : ITicketCreator
{
    IAuthenticationRepository AuthenticationRepository { get; }
    IAuthenticationSession AuthenticationSession { get; }
    IAuthenticationLog AuthenticationLog { get; }
    IHttpContextAccessor HttpContextAccessorAccessor { get; }

    public TicketCreator(IAuthenticationRepository authenticationRepository,
        IAuthenticationSession authenticationSession,
        IAuthenticationLog authenticationLog, IHttpContextAccessor httpContextAccessorAccessor)
    {
        AuthenticationRepository = authenticationRepository;
        AuthenticationSession = authenticationSession;
        AuthenticationLog = authenticationLog;
        HttpContextAccessorAccessor = httpContextAccessorAccessor;
    }
    public async Task<AuthenticationTicket> GetTicket(string userName)
    {
        var user = await AuthenticationRepository.Get(userName) ?? throw new NullReferenceException();

        var headerAuthenticatedId = HttpContextAccessorAccessor.HttpContext?.Request.Headers[CustomHeaders.AuthenticatedId].ToString();
        var authenticatedId = string.IsNullOrEmpty(headerAuthenticatedId) ? await AuthenticationLog.Log(user.UserId) : int.Parse(headerAuthenticatedId);

        AuthenticationSession.AssignAuthentication(authenticatedId);

        var claims = new Collection<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email)
            };
        foreach (var roleName in from fieldsInRoles in typeof(Roles).GetFields()
                                 let fieldName = fieldsInRoles.GetValue(null)?.ToString() ?? string.Empty
                                 where user.Roles.Select(_ => _.RoleName).Contains(fieldName)
                                 select fieldName)
            claims.Add(new Claim(ClaimTypes.Role, roleName));

        var identity = new ClaimsIdentity(claims, nameof(TicketCreator));
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, nameof(TicketCreator));
        return ticket;
    }

}
