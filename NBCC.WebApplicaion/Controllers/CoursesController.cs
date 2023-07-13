/*
 * Note 9
 * Two unique styles might be noticed about this file.  One, the namespace is one
 * line and has no braces.  This is a style change released in newer versions of the
 * framework.  It is again only for esthetics.  And two, the only imported namespaces
 * are custom NBCC classes.  The rest of the imports are located in the GlobalUsing.cs
 * file.  Any using statement that is placed in there with the global reserved word
 * ahead of it makes it accessible to the entire application without needing to
 * specify it in that file.
 * I chose to move only the system and 3rd party import statements and leave the custom
 * imports in the file as a visual guide as to how the files are connected but an
 * argument could be made to move all the imports or none at all.
 * There is no performance consequence for either choice.
 */
using NBCC.Authorization.Models;
using NBCC.Courses.Commands;
using NBCC.Courses.Models;
using NBCC.Courses.Queries;
using NBCC.CQRS.Commands;

namespace NBCC.Courses.WebApplication.Controllers;

/*
 * Note 10
 * Basic setup for web applications has us declaring that this class is
 * a controller with the ApiController attribute and naming of the route.
 * This can be hard coded text or the use of [controller] that will then
 * use the class name minus the "controller" suffix.
 * For example.
 * If this product where deployed to NBCC.com, this code could be reached at
 * NBCC.com/Courses.
 *
 *
 * Finally, a simple use of the ControllerBase extension to give us access
 * to return types and basic web related information and functions.
 *
 */
[Route("[controller]")]
[ApiController]
public sealed class CoursesController : ControllerBase
{
    ICommandDispatcher<int> ValueDispatcher { get; }
    ICommandDispatcher Dispatcher { get; }
    IQueryHandler<CoursesQuery, IEnumerable<Course>> QueryHandler { get; }
    /*
     * Note 11
     * Dispatchers and Handlers:  AKA custom CQRS
     * Another technique in the clean code world is to separate your
     * queries (selects and fetches) from your commands (Update, insert, delete, etc)
     * This is a guideline, not a rule.  In fact, it's important
     * to note that ANYTHING I have done in this solution is just that.
     * Guidelines can and will be broken depending on circumstances.  The
     * important part is to put an effort into following a pattern when
     * possible.
     *
     * In this controller, I have 3 different interactions that will happen:
     * A command that returns an int, a command with no return value and
     * a query that returns a list of courses.
     *
     * In order to attempt to keep these unique interactions  small and clean,
     * I'm using a custom CQRS model.  In my experience, there are lots of
     * 3rd party products that do this but many of them experience DI complications
     * that are difficult to resolve or have too many features that aren't needed.
     *
     * That being said, I'm using a bare bones NBCC CQRS project.  The usage is
     * fairly simple.  Choose which style of dispatcher or query handler you want
     * to use and use the command or query file as it's parameter, passing in any
     * information via constructor parameters as needed.
     *
     * Also to note, as you'll see in almost all my constructors,
     * the "variable ?? throw " line.  This is another short hand for
     * if variable is null, throw an example (which, given the exception type, helps
     * debugging immensely), otherwise, use the given variable for the assignment.
     */

    public CoursesController(ICommandDispatcher<int> valueDispatcher,
        ICommandDispatcher dispatcher,
        IQueryHandler<CoursesQuery, IEnumerable<Course>> queryHandler)
    {
        ValueDispatcher = valueDispatcher ?? throw new ArgumentNullException(nameof(valueDispatcher));
        Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        QueryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
    }

    /*
     * Note 12
     * Describing the functions exposed by the controller.
     * This is more uncommon as you can get away with only
     * the first two lines of the attributes below but here
     * is why I describe it in more detail.  You're developing
     * The this controller, API if you will, for a 3rd party
     * audience.  Even if that audience is sitting directly
     * beside you.  Their only window into the usage of your
     * product is here.  I'm telling my user to expect a
     * response of 201 when the interaction is successful
     * where they might have thought it was 200.
     * They'll know it's a json call, that it requires
     * authorization, and that it will handle errors cleanly
     * with a 500.  You can also expose the return type on
     * queries.  What takes 2 minutes to create offers the
     * audience a wealth of information at a glance.  So do
     * it correct the first time and reduce the need for
     * internal chatter.
     */

    /*
     * Note 12
     * Authorize
     * There are two levels of security I've included in this web application
     * Authentication and authorization.
     * Authentication meaning:  Yes, you can enter my application, usually dependent
     * on a username/password mechanism.
     * And authorization meaning, sure you have credentials to use my application but
     * are you allowed to do this one specific thing.
     * In this example, I have two roles setup in the database.  Administrators and
     * Instructors.  Only members of those two groups are allowed to call this function.
     * Otherwise, a response of 403 will be given.  More on this setup later.
     *
     */
    [HttpPost]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    /*
     * Note 13
     * Basic validation
     * Keeping our audience in mind once again, we're going to let the application
     * explain the expectation instead of the user questioning anything.
     * As the developer who has access to the data source (SQL), I know or can
     * find out that the max length of the course name is 50 characters and that in
     * creating a course, that is a required field. Each property should be
     * described as much as possible.
     */

    /*
     * Note 14
     * Return values for creation functions.
     * The only return value a command should typically have is the ID that was created
     * when it is successfully persisted.  This way, the audience doesn't have to
     * query the database to get the information they just made.  Useful if they're
     * perhaps filling a form or a grid.
     */

    /*
     * Note 15
     * async calls
     * to maximize the server that the web application is deployed to, prefer async
     * calls.  this will make each API call run on it's own thread on the server as
     * opposed to being 'queued' on a synchronous application.  The keyword 'await'
     * will be needed to tell the code to expect an async call.  There may be examples
     * online of people using 'FromResult' or  'Wait' but those will kill the
     * asynchronous thread and result in performance loses.
     */
    public async Task<IActionResult> MakeCourse([Required][MaxLength(50)] string courseName)
    {
        var courseId = await ValueDispatcher.Dispatch(new MakeCoursesCommand(courseName));
        return Created(new Uri(Request.Path, UriKind.Relative), courseId);
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> GetCourses() => Ok(await QueryHandler.Handle(new CoursesQuery()));

    [HttpDelete]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
    public async Task<IActionResult> ArchiveCourse([Required] int courseId)
    {
        await Dispatcher.Dispatch(new ArchiveCoursesCommand(courseId));
        return Ok();
    }
}
