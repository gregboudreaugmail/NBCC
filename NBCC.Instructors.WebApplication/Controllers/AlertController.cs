using NBCC.Authorization.Models;
using NBCC.CQRS.Commands;
using NBCC.Instructors.Commands;
using NBCC.Instructors.Commands.Alerts;

namespace NBCC.Instructors.WebApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        ICommandDispatcher Dispatcher { get; }

        public AlertController(ICommandDispatcher dispatcher) => Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));

        [HttpPost]
        [Authorize(Roles = $"{Roles.Administrator},{Roles.Instructor}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> AlertCourseDeleted(int courseId)
        {
            await Dispatcher.Dispatch(new CourseDeleted(courseId));
            return Ok();
        }
    }
}
