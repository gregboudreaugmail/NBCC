using NBCC.Authorization;
using NBCC.Authorization.CommandHandlers;
using NBCC.Authorization.Commands;
using NBCC.Authorization.DataAccess;
using NBCC.Authorization.Models;
using NBCC.Authorization.Query;
using NBCC.Authorization.QueryHandlers;
using NBCC.Authorization.ServiceExtentions;
using NBCC.Authorization.WebApplication.Messages;
using NBCC.CQRS.Commands;
using NBCC.Middleware;
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
        services.AddAuthentication(BasicAuthentication)
         .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthentication, null);
        services.TryAddSingleton(new AuthorizationConnection(Configuration["ConnectionStrings:Connection"] ?? ""));
        services.TryAddSingleton(new LoggingConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.AddSingleton(Configuration.GetRequiredSection(nameof(Message))
                .Get<Message>() ?? new Message());
    }

    public void Configure(IApplicationBuilder app, Message message)
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
