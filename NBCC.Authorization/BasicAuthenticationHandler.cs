using NBCC.Authorization.DataAccess;
using System.Collections.ObjectModel;

namespace NBCC.Authorization;

public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private ILoggerFactory LoggerFactory { get; }
    IAuthenticationRepository AuthenticationRepository { get; }

    public BasicAuthenticationHandler(IAuthenticationRepository authenticationRepository,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, loggerFactory, encoder, clock)
    {
        LoggerFactory = loggerFactory;
        AuthenticationRepository = authenticationRepository;
    }
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? userName;
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter ?? string.Empty)).Split(':');
            userName = credentials.FirstOrDefault() ?? string.Empty;
            var password = credentials.LastOrDefault() ?? string.Empty;
            
            var authenticated = await AuthenticationRepository.AuthenticateUser(userName, password);

            if (!authenticated) throw new ArgumentException("Invalid credentials");
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
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email)
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
