using NBCC.Courses.Commands;
using System.ComponentModel.DataAnnotations;

namespace NBCC.WebApplicaion.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class CoursesController : ControllerBase
{
    ICommandDispatcher Messages { get; }
    public CoursesController(ICommandDispatcher messages) => Messages = messages ?? throw new ArgumentNullException(nameof(messages));

    [HttpPost]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<IActionResult> Post([Required][MaxLength(50)] string courseName)
    {
        await Messages.Dispatch(new CoursesCommand(courseName));
        return Ok();
    }
}
