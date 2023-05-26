using System.Net.Mime;

namespace NBCC.Instructors.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class InstructorsController : ControllerBase
{
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task Post( CourseAssignment courseId)
    {
        var x = 0;

    }
}

public class CourseAssignment
{
    public int  CourseId { get; set; }
    public int InstructorId { get; set; }
}