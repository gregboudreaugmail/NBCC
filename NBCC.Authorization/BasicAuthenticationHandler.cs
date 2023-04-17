using NBCC.Authorizaion.DataAccess;
using System.Collections.ObjectModel;
using System.Linq;

namespace NBCC.Authorization;

public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    IAuthenticationRepository AuthenticationRepository { get; }

    public BasicAuthenticationHandler(IAuthenticationRepository authenticationRepository,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock) => AuthenticationRepository = authenticationRepository;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string userName = string.Empty;
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader == null)
                return await Task.FromResult(AuthenticateResult.Fail("Authentication failed"));

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter ?? string.Empty)).Split(':');
            userName = credentials.FirstOrDefault() ?? string.Empty;
            var password = credentials.LastOrDefault() ?? string.Empty;

            if (!await AuthenticationRepository.AuthenticateUser(userName, password)) throw new ArgumentException("Invalid credentials");
        }
        catch (Exception ex)
        {
            return await Task.FromResult(AuthenticateResult.Fail($"Authentication failed: {ex.Message}"));
        }

        return await Task.FromResult(AuthenticateResult.Success(await GetUser(userName)));
    }

    private async Task<AuthenticationTicket> GetUser(string userName)
    {
        var user = await AuthenticationRepository.GetUser(userName) ?? throw new NullReferenceException();
        var claims = new Collection<Claim> {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.UserName),
        };
        foreach (var v in from p in typeof(Roles).GetFields()
                          let v = p.GetValue(null)?.ToString() ?? string.Empty
                          where user.Roles.Select(_ => _.RoleName).Contains(v)
                          select v)
            claims.Add(new Claim(ClaimTypes.Role, v));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return ticket;
    }
}
public static class Roles
{
    public const string Administrator = nameof(Administrator);
    public const string Instructor = nameof(Instructor);
}
