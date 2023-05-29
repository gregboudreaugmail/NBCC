using NBCC.Authorization;
using NBCC.Authorization.DataAccess;
using NBCC.Authorization.Models;
using NBCC.Authorization.ServiceExtentions;
using NBCC.CQRS.Commands;
using NBCC.Instructors.Commands;
using NBCC.Instructors.DataAccess;
using NBCC.Logging;
using NBCC.Logging.DataAccess;
using NBCC.Logging.Models;
using NBCC.Middleware;
using NBCC.Middleware.Messages;
using NBCC.WebRequest;
using AuthorizationConnection = NBCC.Authorization.DataAccess.Connection;
using LoggingConnection = NBCC.Logging.DataAccess.Connection;
using InstructorsConnection = NBCC.Instructors.DataAccess.Connection;
using NBCC.Instructors.CommandHandlers;
using NBCC.Instructors.Models;
using NBCC.Instructors.Queries;
using NBCC.Instructors.QueryHandlers;

namespace NBCC.Instructors.WebApplication;

public class Startup
{
    const string BasicAuthentication = "BasicAuthentication";

    IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddTransient<IInstructorRepository, InstructorRepository>();
        services.AddTransient<ICommandHandler<AddInstructorCommand, int>, AddInstructorCommandHandler>();
        services.AddTransient<IQueryHandler<InstructorsQuery, IEnumerable<Instructor>>, InstructorsQueryHandler>();
        services.AddTransient<ICommandHandler<ArchiveInstructorCommand>, ArchiveInstructorCommandHandler>();
        services.AddTransient<ITicketCreator, TicketCreator>();
        services.AddControllers();
        services.AddHttpClient();
        services.AddTransient<IPost, Post>();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddTransient<IUser, User>();
        services.AddTransient<ILoggerAsync, LoggerAsync>();
        services.AddSwaggerGen(OpenApi.AddAuthentication());
        services.AddTransient<IExceptionLog, ExceptionLog>();
        services.AddTransient<IInteractionLog, InteractionLog>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.AddTransient<IAuthenticationLog, AuthenticationLog>();
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<ICommandDispatcher<int>, CommandDispatcher<int>>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
        services.AddTransient<IAuthenticationSession, AuthenticationSession>();
        services.TryAddSingleton(new LoggingConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.TryAddSingleton(new InstructorsConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));

        services.TryAddSingleton(Configuration.
            GetSection("Messages").Get<ErrorMessage>() ?? new ErrorMessage());

        services.TryAddSingleton(new AuthorizationConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.AddAuthentication(BasicAuthentication)
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthentication, null);
    }

    public void Configure(IApplicationBuilder app, ErrorMessage errorMessage)
    {
        app.UseCustomMiddleware(errorMessage)
            .UseSwagger()
            .UseSwaggerUI()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(_ => _.MapControllers());
    }
}
