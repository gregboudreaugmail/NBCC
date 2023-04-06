using Microsoft.Extensions.DependencyInjection.Extensions;
using NBCC.Authorizaion;
using NBCC.Courses.CommandHandlers;
using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;

namespace NBCC.WebApplicaion;

public class Startup
{
    const string BASIC_AUTHENTICATION = "BasicAuthentication";

    IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(OpenApi.AddAuthentication());
        services.AddAuthentication(BASIC_AUTHENTICATION)
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BASIC_AUTHENTICATION, null);

        services.AddLogging();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();
        services.AddHttpContextAccessor();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.TryAddSingleton(new Connection(Configuration["ConnectionStrings:Connection"] ?? ""));
        services.AddTransient<ICourseRepository, CourseRepository>();
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.AddTransient<ICommandHandler<CoursesCommand>, CoursesCommandHandler>();

    }
    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger()
           .UseSwaggerUI()
           .UseRouting()
           .UseAuthentication()
           .UseAuthorization()
           .UseEndpoints(_ => _.MapControllers())
           .UseCustomMiddleware();
    }
}
