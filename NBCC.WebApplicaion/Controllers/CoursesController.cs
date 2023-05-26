using NBCC.Authorization.Models;
using NBCC.Courses.Commands;
using NBCC.CQRS.Commands;

namespace NBCC.Courses.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class CoursesController : ControllerBase
{
    ICommandDispatcher<int> ValueDispatcher { get; }
    ICommandDispatcher Dispatcher { get; }

    public CoursesController(ICommandDispatcher<int> valueDispatcher, ICommandDispatcher dispatcher)
    {
        ValueDispatcher = valueDispatcher ?? throw new ArgumentNullException(nameof(valueDispatcher));
        Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
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
    public async Task<IActionResult> MakeCourse([Required][MaxLength(50)] string courseName) =>
        await MakeCourse(courseName, null);

    [HttpPost("WithAssignment")]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<IActionResult> MakeCourse([Required][MaxLength(50)] string courseName, int? instructorId)
    {
        var courseId = await ValueDispatcher.Dispatch(new MakeCoursesCommand(courseName, instructorId));
        return Created(new Uri(Request.Path, UriKind.Relative), courseId);
    }

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
