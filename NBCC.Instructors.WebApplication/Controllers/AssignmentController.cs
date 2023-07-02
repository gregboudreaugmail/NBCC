using NBCC.Authorization.Models;
using NBCC.CQRS.Commands;
using NBCC.Instructors.Commands.Assignments;
using NBCC.Instructors.Models;
using NBCC.Instructors.Queries;

namespace NBCC.Instructors.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class AssignmentController : ControllerBase
{
    ICommandDispatcher<int> ValueDispatcher { get; }
    ICommandDispatcher Dispatcher { get; }
    IQueryHandler<AssignmentsQuery, IEnumerable<Assignment>> QueryHandler { get; }

    public AssignmentController(ICommandDispatcher<int> valueDispatcher,
        ICommandDispatcher dispatcher,
        IQueryHandler<AssignmentsQuery, IEnumerable<Assignment>> queryHandler)
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
    public async Task<IActionResult> AddAssignment([Required] int instructorId, [Required] int courseId)
    {
        var assignmentId = await ValueDispatcher.Dispatch(new AddAssignmentCommand(instructorId, courseId));
        return Created(new Uri(Request.Path, UriKind.Relative), assignmentId);
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> GetAssignments([Required] int instructorId) => Ok(await QueryHandler.Handle(new AssignmentsQuery(instructorId)));

    [HttpDelete]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
    public async Task<IActionResult> ArchiveAssignments([Required] int instructorId, [Required] int courseId)
    {
        await Dispatcher.Dispatch(new ArchiveAssignmentsCommand(instructorId, courseId));
        return Ok();
    }
}
