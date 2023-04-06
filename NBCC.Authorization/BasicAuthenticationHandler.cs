using NBCC.Authorizaion;

namespace NBCC.Authorization;

public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    IUserService UserService { get; }

    public BasicAuthenticationHandler(IUserService userService,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        UserService = userService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string username;
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader == null)
            {
                return await Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
            }

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter ?? string.Empty)).Split(':');
            username = credentials.FirstOrDefault() ?? string.Empty;
            var password = credentials.LastOrDefault() ?? string.Empty;

            if (!UserService.ValidateCredentials(username, password))
                throw new ArgumentException("Invalid credentials");
        }
        catch (Exception ex)
        {
            return await Task.FromResult(AuthenticateResult.Fail($"Authentication failed: {ex.Message}"));
        }

        var claims = new[] {
            new Claim(ClaimTypes.Name, username)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }

}