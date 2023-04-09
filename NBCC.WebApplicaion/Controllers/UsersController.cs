using NBCC.Authorizaion.Commands;
using NBCC.Courses.Commands;
using System.ComponentModel.DataAnnotations;

namespace NBCC.WebApplicaion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        ICommandDispatcher Messages { get; }
        public UsersController(ICommandDispatcher messages) => Messages = messages ?? throw new ArgumentNullException(nameof(messages));

        [HttpPost]
        [Authorize(Roles = Roles.Administrators)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> Post([Required][MaxLength(50)] string userName, [Required][MaxLength(50)] string password)
        {
            await Messages.Dispatch(new UserCommand(userName, password));
            return Created(new Uri(Request.Path, UriKind.Relative), userName);
        }
    }
}
