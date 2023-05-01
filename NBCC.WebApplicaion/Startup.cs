using Microsoft.Extensions.DependencyInjection.Extensions;
using NBCC.Authorization.DataAccess;
using NBCC.Courses.CommandHandlers;
using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;
using NBCC.Courses.WebApplication.Messages;
using NBCC.Logging;
using NBCC.Logging.DataAccess;
using NBCC.Logging.Models;
using NBCC.WebApplication;
using Connection = NBCC.Logging.DataAccess.Connection;

namespace NBCC.Courses.WebApplication;

public class Startup
{
    const string BasicAuthentication = "BasicAuthentication";

    IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddControllers();
        services.AddDistributedMemoryCache();
        services.AddSession();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddTransient<IUser, User>();
        services.AddSwaggerGen(OpenApi.AddAuthentication());
        services.AddTransient<IInteractionLog, InteractionLog>();
        services.AddTransient<IAuthenticationSession, AuthenticationSessionSession>();
        services.AddTransient<ICourseRepository, CourseRepository>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.AddSingleton<ILoggerProvider, CustomLoggerProvider>();
        services.AddTransient<IAuthenticationLog, AuthenticationLog>();
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
        services.AddTransient<ICommandHandler<CoursesCommand>, CoursesCommandHandler>();
        services.TryAddSingleton(new Connection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.TryAddSingleton(Configuration.GetRequiredSection(nameof(Message)).Get<Message>() ?? new Message());
        services.TryAddSingleton(new DataAccess.Connection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.TryAddSingleton(new AuthenticationConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.AddAuthentication(BasicAuthentication).AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthentication, null);
    }

    public void Configure(IApplicationBuilder app, Message message)
    {
        app.UseCustomMiddleware(message)
            .UseSwagger()
            .UseSession()
            .UseSwaggerUI()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(_ => _.MapControllers());
    }
}
