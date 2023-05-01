using NBCC.Authorization.DataAccess;
using NBCC.Logging.DataAccess;
using NBCC.Logging.Models;
using System.Collections.ObjectModel;

namespace NBCC.Authorization;

public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    IAuthenticationRepository AuthenticationRepository { get; }
    IAuthenticationSession AuthenticationSession { get; }
    IAuthenticationLog AuthenticationLog { get; }

    public BasicAuthenticationHandler(IAuthenticationRepository authenticationRepository,
        IAuthenticationSession authenticationSession,
        IAuthenticationLog authenticationLog,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, loggerFactory, encoder, clock)
    {
        AuthenticationRepository = authenticationRepository;
        AuthenticationSession = authenticationSession;
        AuthenticationLog = authenticationLog;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? userName;
        try
        {
            userName = ExtractUserNameAndPassword(out var password);

            var authenticated = await AuthenticationRepository.AuthenticateUser(userName, password);
            if (!authenticated) throw new ArgumentException("Invalid credentials");
        }
        catch (Exception ex)
        {
            return await Task.FromResult(AuthenticateResult.Fail($"AuthenticationSessionSession failed: {ex.Message}"));
        }

        return await Task.FromResult(AuthenticateResult.Success(await GetTicket(userName)));


        string ExtractUserNameAndPassword(out string password)
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

            var credentials = Encoding.UTF8.GetString(
                Convert.FromBase64String(authHeader.Parameter ?? string.Empty)).Split(':');
            password = credentials.LastOrDefault() ?? string.Empty;
            return credentials.FirstOrDefault() ?? string.Empty;
        }
    }

    private async Task<AuthenticationTicket> GetTicket(string userName)
    {
        var user = await AuthenticationRepository.GetUser(userName) ?? throw new NullReferenceException();

        var authenticatedId = await LogAuthentication(user);

        var claims = new Collection<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(CustomClaimTypes.AuthenticationLogId, authenticatedId.ToString()),
        };
        foreach (var roleName in from fieldsInRoles in typeof(Roles).GetFields()
                 let fieldName = fieldsInRoles.GetValue(null)?.ToString() ?? string.Empty
                 where user.Roles.Select(_ => _.RoleName).Contains(fieldName)
                 select fieldName)
            claims.Add(new Claim(ClaimTypes.Role, roleName));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return ticket;
    }

    private async Task<int> LogAuthentication(IUser user)
    {
        try
        {
            var authenticatedId = await AuthenticationLog.Log(user.UserId);
            AuthenticationSession.AssignAuthentication(authenticatedId, user.UserId);
            return authenticatedId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
    }
}