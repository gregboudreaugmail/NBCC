using NBCC.Courses.Commands;
using System.ComponentModel.DataAnnotations;

namespace NBCC.Courses.WebApplicaion.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class CoursesController : ControllerBase
{
    ICommandDispatcher Messages { get; }
    public CoursesController(ICommandDispatcher messages) => Messages = messages ?? throw new ArgumentNullException(nameof(messages));

    [HttpPost]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<IActionResult> Post(
        [Required]
        [Display(Name = "Course Name")]
        [MaxLength(50)] 
            string courseName)
    {
        await Messages.Dispatch(new CoursesCommand(courseName));
        return Created(new Uri(Request.Path, UriKind.Relative), courseName);
    }
}
