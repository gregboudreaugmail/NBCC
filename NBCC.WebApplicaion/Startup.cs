﻿using Microsoft.Extensions.DependencyInjection.Extensions;
using NBCC.Authorizaion;
using NBCC.Authorizaion.DataAccess;
using NBCC.Courses.CommandHandlers;
using NBCC.Courses.Commands;
using NBCC.Courses.DataAccess;
using NBCC.Courses.WebApplication.Messages;
using NBCC.Logs.DataAccess;
using NBCC.WebApplicaion;

namespace NBCC.Courses.WebApplicaion;

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
        services.AddSwaggerGen(OpenApi.AddAuthentication());
        services.AddTransient<ICourseRepository, CourseRepository>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<ICommandHandler<CoursesCommand>, CoursesCommandHandler>();
        services.TryAddSingleton(new Connection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.TryAddSingleton(new AuthenticationConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.TryAddSingleton(new LoggingConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.AddTransient<IAuthenticatedUser, AuthenticatedUser>();
        services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
        services.AddAuthentication(BASIC_AUTHENTICATION)
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BASIC_AUTHENTICATION, null);
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
