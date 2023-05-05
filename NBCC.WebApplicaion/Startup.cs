using Microsoft.Extensions.DependencyInjection.Extensions;
using NBCC.Authorization.DataAccess;
using NBCC.Courses.CommandHandlers;
using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;
using NBCC.Courses.WebApplication.Messages;
using NBCC.Logging.DataAccess;
using NBCC.Logging.Models;
using NBCC.WebApplication;
using LoggingConnection = NBCC.Logging.DataAccess.Connection;
using CoursesConnection = NBCC.Courses.DataAccess.Connection;
using AuthorizationConnection = NBCC.Authorization.DataAccess.Connection;
using Microsoft.Extensions.Caching.Distributed;

namespace NBCC.Courses.WebApplication;

public class Startup
{
    const string BasicAuthentication = "BasicAuthentication";

    IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(60);
        });
        services.AddMemoryCache();
        services.AddControllers();
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
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
        services.AddTransient<IAuthenticationSession, AuthenticationSession>();
        services.AddTransient<ICommandHandler<CoursesCommand>, CoursesCommandHandler>();
        services.TryAddSingleton(new LoggingConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.TryAddSingleton(new CoursesConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.TryAddSingleton(Configuration.GetRequiredSection(nameof(Message)).Get<Message>() ?? new Message());
        services.TryAddSingleton(new AuthorizationConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.AddAuthentication(BasicAuthentication)
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthentication, null);
    }

    public void Configure(IApplicationBuilder app, Message message)
    {
        app.UseCustomMiddleware(message)
            .UseSwagger()
            .UseSwaggerUI()
            .UseRouting()
            .UseSession()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(_ => _.MapControllers());
    }
}
