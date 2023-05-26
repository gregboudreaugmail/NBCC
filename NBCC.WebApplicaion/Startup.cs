using NBCC.Authorization.DataAccess;
using NBCC.Authorization.Models;
using NBCC.Authorization.ServiceExtentions;
using NBCC.Courses.CommandHandlers;
using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;
using NBCC.Courses.Models;
using NBCC.Courses.Queries;
using NBCC.Courses.QueryHandlers;
using NBCC.CQRS.Commands;
using NBCC.Logging;
using NBCC.Logging.DataAccess;
using NBCC.Logging.Models;
using NBCC.Middleware;
using NBCC.Middleware.Messages;
using NBCC.WebRequest;
using AuthorizationConnection = NBCC.Authorization.DataAccess.Connection;
using CoursesConnection = NBCC.Courses.DataAccess.Connection;
using LoggingConnection = NBCC.Logging.DataAccess.Connection;

namespace NBCC.Courses.WebApplication;

public class Startup
{
    const string BasicAuthentication = "BasicAuthentication";

    IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {

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
        services.AddTransient<ICourseRepository, CourseRepository>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.AddTransient<IAuthenticationLog, AuthenticationLog>();
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<ICommandDispatcher<int>, CommandDispatcher<int>>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
        services.AddTransient<IAuthenticationSession, AuthenticationSession>();
        services.AddTransient<ICommandHandler<MakeCoursesCommand, int>, MakeCoursesCommandHandler>();
        services.AddTransient<IQueryHandler<CoursesQuery, IEnumerable<Course>>, CoursesQueryHandler>();
        services.AddTransient<ICommandHandler<ArchiveCoursesCommand>, ArchiveCoursesCommandHandler>();
        services.TryAddSingleton(new LoggingConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.TryAddSingleton(new CoursesConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));

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
