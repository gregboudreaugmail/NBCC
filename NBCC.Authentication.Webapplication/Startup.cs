using Microsoft.AspNetCore.Authentication;
using NBCC.Authorizaion;
using NBCC.Authorizaion.CommandHandlers;
using NBCC.Authorizaion.Commands;
using NBCC.Authorizaion.DataAccess;
using NBCC.Authorizaion.Query;
using NBCC.Authorizaion.QueryHandlers;
using NBCC.Authorization;
using NBCC.Courses.Commands;

namespace NBCC.Authentication.Webapplication
{
    public class Startup
    {
        const string BASIC_AUTHENTICATION = "BasicAuthentication";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRolesRepository, RolesRepository>();
            services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();
            services.AddTransient<ICommandHandler<UserCommand>, UserCommandHandler>();
            services.AddTransient<IQueryHandler<RolesQuery, IEnumerable<Role>>, RolesQueryHandler>();
            services.AddAuthentication(BASIC_AUTHENTICATION)
             .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BASIC_AUTHENTICATION, null);

        }
    }
}