using Microsoft.AspNetCore.Mvc.Filters;
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
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<IActionResult> Post([Required][MaxLength(50)] string courseName)
    {
        await Messages.Dispatch(new CoursesCommand(courseName));
        return Created(new Uri(Request.Path, UriKind.Relative), courseName);
    }
}
