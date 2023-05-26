using NBCC.Authorization.Models;
using NBCC.Authorization.Query;
using NBCC.Courses.Commands;
using NBCC.Courses.Models;
using NBCC.Courses.Queries;
using NBCC.CQRS.Commands;

namespace NBCC.Courses.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class CoursesController : ControllerBase
{
    ICommandDispatcher<int> ValueDispatcher { get; }
    ICommandDispatcher Dispatcher { get; }
    IQueryHandler<CoursesQuery, IEnumerable<Course>> QueryHandler { get; }

    public CoursesController(ICommandDispatcher<int> valueDispatcher,
        ICommandDispatcher dispatcher,
        IQueryHandler<CoursesQuery, IEnumerable<Course>> queryHandler)
    {
        ValueDispatcher = valueDispatcher ?? throw new ArgumentNullException(nameof(valueDispatcher));
        Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        QueryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
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
