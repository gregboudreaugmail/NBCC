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
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddScoped<IUserService, UserService>();
        services.AddSwaggerGen(OpenApi.AddAuthentication());
        services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();
        services.AddTransient<ICourseRepository, CourseRepository>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<ICommandHandler<CoursesCommand>, CoursesCommandHandler>();
        services.TryAddSingleton(new Connection(Configuration["ConnectionStrings:Connection"] ?? ""));
        services.AddAuthentication(BASIC_AUTHENTICATION)
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BASIC_AUTHENTICATION, null);
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
