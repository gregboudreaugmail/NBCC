using NBCC.Authorization;
using NBCC.Authorization.CommandHandlers;
using NBCC.Authorization.Commands;
using NBCC.Authorization.DataAccess;
using NBCC.Authorization.Models;
using NBCC.Authorization.Query;
using NBCC.Authorization.QueryHandlers;
using NBCC.Authorization.ServiceExtentions;
using NBCC.CQRS.Commands;
using NBCC.Logging.DataAccess;
using NBCC.Logging.Models;
using NBCC.Logging;
using NBCC.Middleware;
using NBCC.Middleware.Messages;
using AuthorizationConnection = NBCC.Authorization.DataAccess.Connection;
using LoggingConnection = NBCC.Logging.DataAccess.Connection;

namespace NBCC.Authentication.WebApplication;

public class Startup
{
    const string BasicAuthentication = "BasicAuthentication";
    IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ITicketCreator, TicketCreator>();
        services.AddControllers();
        services.AddSwaggerGen(OpenApi.AddAuthentication());
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRolesRepository, RolesRepository>();
        services.AddTransient<IUser, User>();
        services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
        services.AddTransient<ICommandHandler<UserCommand>, UserCommandHandler>();
        services.AddTransient<IQueryHandler<RolesQuery, IEnumerable<Role>>, RolesQueryHandler>();
        services.TryAddSingleton(new AuthorizationConnection(Configuration["ConnectionStrings:Connection"] ?? ""));
        services.TryAddSingleton(new LoggingConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.TryAddSingleton(Configuration.
            GetRequiredSection("Messages").Get<ErrorMessage>() ?? new ErrorMessage());
        services.AddTransient<ILoggerAsync, LoggerAsync>();
        services.AddTransient<IExceptionLog, ExceptionLog>();
        services.AddTransient<IInteractionLog, InteractionLog>();
        services.AddTransient<IAuthenticationLog, AuthenticationLog>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IAuthenticationSession, AuthenticationSession>();
        services.AddAuthentication(BasicAuthentication)
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthentication, null);
    }

    public void Configure(IApplicationBuilder app, ErrorMessage message)
    {
        app.UseCustomMiddleware(message)
           .UseSwagger()
           .UseSwaggerUI()
           .UseRouting()
           .UseAuthentication()
           .UseAuthorization()
           .UseEndpoints(_ => _.MapControllers());
    }
}
