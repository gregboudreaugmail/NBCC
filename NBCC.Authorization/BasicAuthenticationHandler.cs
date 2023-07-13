using NBCC.Authorization.DataAccess;
using NBCC.Authorization.Models;

namespace NBCC.Authorization;

/*
 * Note 25
 * Authentication
 * The blueprint for authentication.  Inherit the AuthenticationHandler, call HandleAuthenticateAsync
 * and return an AuthenticateResult.  The logic in which you determine it's a pass or fails is
 * on you though.
 */
public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    IAuthenticationRepository AuthenticationRepository { get; }
    ITicketCreator TicketCreator { get; }
    ILoggerAsync LoggerAsync { get; }
    User? AuthorizedUser { get; set; }

    public BasicAuthenticationHandler(IAuthenticationRepository authenticationRepository,
        ITicketCreator ticketCreator, 
        ILoggerAsync loggerAsync,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, loggerFactory, encoder, clock)
    {
        AuthenticationRepository = authenticationRepository ?? throw new ArgumentNullException(nameof(authenticationRepository));
        TicketCreator = ticketCreator ?? throw new ArgumentNullException(nameof(ticketCreator));
        LoggerAsync = loggerAsync ?? throw new ArgumentNullException(nameof(loggerAsync));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        LoggerAsync.Log(new UnauthorizedAccessException($"Username {AuthorizedUser?.UserName}"));
        return base.HandleChallengeAsync(properties);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            /*
             * Note 26
             * Verifying the username and password
             * We're going to be storing our password in the database encrypted which means this
             * won't be a one to one match.  Instead we're going to pull the saved password via the
             * email, encrypt our current password passed in through the login and see if THEY match.
             * Note that auth isn't a traditional controller/body style call. The information is put
             * into the header so the below code is extracting the given credentials from the header
             */
            AuthorizedUser = ExtractUserNameAndPassword();

            var authenticated = await AuthenticationRepository.AuthenticateUser(AuthorizedUser.UserName, AuthorizedUser.Password);
            if (!authenticated) throw new ArgumentException(string.Empty);

            return await Task.FromResult(AuthenticateResult.Success(await TicketCreator.GetTicket(AuthorizedUser.UserName)));
        }
        catch (Exception)
        {
            return await Task.FromResult(AuthenticateResult.Fail(string.Empty));
        }
    }
    User ExtractUserNameAndPassword()
    {
        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

        var credentials = Encoding.UTF8.GetString(
            Convert.FromBase64String(authHeader.Parameter ?? string.Empty)).Split(':');
        return new User(credentials.FirstOrDefault() ?? string.Empty, credentials.LastOrDefault() ?? string.Empty);
    }
    record User(string UserName, string Password);
}
