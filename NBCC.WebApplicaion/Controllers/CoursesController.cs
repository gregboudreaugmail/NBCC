using NBCC.Courses.Commands;

namespace NBCC.Courses.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class CoursesController : ControllerBase
{
    ICommandDispatcher Dispatcher { get; }
    public CoursesController(ICommandDispatcher dispatcher) => Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));

    [HttpPost]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<IActionResult> Post([Required][MaxLength(50)] string courseName)
    {
        await Dispatcher.Dispatch(new CoursesCommand(courseName));
        return Created(new Uri(Request.Path, UriKind.Relative), courseName);
    }
}
