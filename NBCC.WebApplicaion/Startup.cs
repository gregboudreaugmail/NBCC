/*
 Note 2
Like the program.cs, this is rum once at startup as the name would have it.
Unlike most classes that, in the clean code world, are "restricted" to be
50 lines or less, this file will either grow or be extracted to a point where
it becomes too difficult to navigate.  Several times, I've seen my co-workers
struggle to find how classes are configured so for that reason, I've allowed
the rule bending.  As the team matures, this would be a prime example of
what to refactor first.
 */
using NBCC.Authorization.DataAccess;
using NBCC.Authorization.Models;
using NBCC.Authorization.ServiceExtensions;
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
    /*
     * Note 3
     * Establishing a pattern for variables is key.  The team must also buy in
     * and adhere to the pattern.  Great opportunity to accomplish this during
     * the pull request (buddy check) process.
     * My preferred pattern is:
     * Most, if not all "string" data will be pulled to the top of the file ane
     * set as a const using pascal casing.
     * I drop the use of "private" for functions and variables as it's redundant.
     * I prefer a property with only a set setter over a readonly variable.
     * Assume a variable or function is private unless otherwise needed.
     * Private functions should have longer, descriptive names.
     * Public functions should have shorter, intuitive names.
     * Private variables should have shorter names, given the smaller scope
     * Public variables should have longer names to show intent of use
     */
    const string BasicAuthentication = "BasicAuthentication";

    IConfiguration Configuration { get; }

    /*
     * Note 4
     * Newer framework applications will allow the use of "lambda functions" which is nothing
     * more than dropping the curled braces and replacing it with an arrow function ( => )
     * This should only be used for functions with 1 line of code.  Nothing more than
     * esthetics though.
     */

    /*
     * Note 5
     * As noted in the note 1, the function to set up the application is the startup file.
     * The configuration variable will allow you access to read the appsettings file.
     * So the constructor and configure functions are optional depending on your needs,
     * however ConfigureServices is required.
     */
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        /*
         * Note 6
         * Dependency Injection (DI).
         * Most of the code below will be configuring what's called dependency injection.
         * It's a common practice today in many frameworks and even different languages.
         * DI plays a major factor in clean code as it allows you to substitute one implementation
         * for another.
         *
         * For example.
         * The first line beneath the comment reads as follows:  Any time that ICourseRepository
         * is referenced via a class constructor (and therefore made into a variable), it will
         * use the class CourseRepository.
         * Why is this important?  CourseRepository is setup to use SQL right now.  But if
         * the infrastructure were to require a change to, lets say to Oracle, the change of
         * code would only be to change this dependency.
         * More so, during the writing of tests (unit tests), you can avoid having to interact
         * with a live database and assign a mock class to ICourseRepository.
         *
         */
        services.AddTransient<ICourseRepository, CourseRepository>();
        services.AddTransient<ICommandHandler<MakeCoursesCommand, int>, MakeCoursesCommandHandler>();
        services.AddTransient<IQueryHandler<CoursesQuery, IEnumerable<Course>>, CoursesQueryHandler>();
        services.AddTransient<ICommandHandler<ArchiveCoursesCommand>, ArchiveCoursesCommandHandler>();
        services.TryAddSingleton(Configuration.
            GetSection(nameof(Alerts)).Get<Alerts>() ?? new Alerts());
        
        /*
         * Note 7
         * Above are the configuration for this application specifically.
         * Below is "boilerplate" code that will appear in all the applications.
         * As stated before, this would be a good spot to extract.
         */
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
        services.TryAddSingleton(new CoursesConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));

        services.TryAddSingleton(Configuration.
            GetSection("Messages").Get<ErrorMessage>() ?? new ErrorMessage());
        
        services.TryAddSingleton(new AuthorizationConnection(Configuration["ConnectionStrings:Connection"] ?? string.Empty));
        services.AddAuthentication(BasicAuthentication)
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthentication, null);
    }

    /*
     * Note 8:
     * While the above function handles DI and appsettings setup, and is fairly free of coding
     * logic, below is the configuration for the application.  Assigned interfaces are available
     * in this function.  The only 'unusual' code here is the middleware line.  The rest is
     * pretty standard for an authorized/authenticated web application using OpenApi (swagger)
     */
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
