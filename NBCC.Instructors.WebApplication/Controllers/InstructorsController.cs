using NBCC.Authorization.Models;
using NBCC.Instructors.Commands;
using NBCC.Instructors.Models;
using NBCC.Instructors.Queries;
using NBCC.CQRS.Commands;

namespace NBCC.Instructors.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class InstructorsController : ControllerBase
{
    ICommandDispatcher<int> ValueDispatcher { get; }
    ICommandDispatcher Dispatcher { get; }
    IQueryHandler<InstructorsQuery, IEnumerable<Instructor>> QueryHandler { get; }

    public InstructorsController(ICommandDispatcher<int> valueDispatcher,
        ICommandDispatcher dispatcher,
        IQueryHandler<InstructorsQuery, IEnumerable<Instructor>> queryHandler)
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
    public async Task<IActionResult> AddInstructor([Required][MaxLength(50)] string firstName, [Required][MaxLength(50)] string lastName, [Required][MaxLength(255)] string email)
    {
        var instructorId = await ValueDispatcher.Dispatch(new AddInstructorCommand(firstName, lastName, email));
        return Created(new Uri(Request.Path, UriKind.Relative), instructorId);
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> GetInstructors() => Ok(await QueryHandler.Handle(new InstructorsQuery()));

    [HttpDelete]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
    public async Task<IActionResult> ArchiveInstructor([Required] int instructorId)
    {
        await Dispatcher.Dispatch(new ArchiveInstructorCommand(instructorId));
        return Ok();
    }
}
