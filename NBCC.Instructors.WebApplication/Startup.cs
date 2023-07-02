using NBCC.Alerts;
using NBCC.Authorization;
using NBCC.Authorization.DataAccess;
using NBCC.Authorization.Models;
using NBCC.Authorization.ServiceExtentions;
using NBCC.CQRS.Commands;
using NBCC.Instructors.CommandHandlers;
using NBCC.Instructors.CommandHandlers.Alerts;
using NBCC.Instructors.CommandHandlers.Assignments;
using NBCC.Instructors.Commands;
using NBCC.Instructors.Commands.Alerts;
using NBCC.Instructors.Commands.Assignments;
using NBCC.Instructors.DataAccess;
using NBCC.Instructors.DataAccess.Assignments;
using NBCC.Instructors.Models;
using NBCC.Instructors.Queries;
using NBCC.Instructors.QueryHandlers;
using NBCC.Logging;
using NBCC.Logging.DataAccess;
using NBCC.Logging.Models;
using NBCC.Middleware;
using NBCC.Middleware.Messages;
using NBCC.WebRequest;
using AuthorizationConnection = NBCC.Authorization.DataAccess.Connection;
using InstructorsConnection = NBCC.Instructors.DataAccess.Connection;
using LoggingConnection = NBCC.Logging.DataAccess.Connection;

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
        services.AddTransient<ICommandHandler<AddAssignmentCommand, int>, AddAssignmentCommandHandler>();
        services.AddTransient<IQueryHandler<InstructorsQuery, IEnumerable<Instructor>>, InstructorsQueryHandler>();
        services.AddTransient<IQueryHandler<AssignmentsQuery, IEnumerable<Assignment>>, AssignmentsQueryHandler>();
        services.AddTransient<ICommandHandler<ArchiveInstructorCommand>, ArchiveInstructorCommandHandler>();
        services.AddTransient<ICommandHandler<ArchiveAssignmentsCommand>, ArchiveAssignmentsCommandHandler>();
        services.AddTransient<ICommandHandler<CourseDeleted>, CourseDeletedCommandHandler>();
        services.AddTransient<IAssignmentRepository, AssignmentRepository>();
        services.AddTransient<IAlerts, Email>();
        services.TryAddSingleton(Configuration.
            GetSection("SmtpAlerts").Get<SmtpAlerts>() ?? new SmtpAlerts());

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
