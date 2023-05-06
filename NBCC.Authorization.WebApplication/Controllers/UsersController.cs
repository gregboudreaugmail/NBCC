using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBCC.Authorization.Commands;
using NBCC.Authorization;
using NBCC.Courses.Commands;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace NBCC.WebApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public sealed class UsersController : ControllerBase
    {
        ICommandDispatcher Messages { get; }
        public UsersController(ICommandDispatcher messages) => Messages = messages ?? throw new ArgumentNullException(nameof(messages));

        [HttpPost]
        [Authorize(Roles = Roles.Administrator)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> Post([MaxLength(50)] string userName,
            [Required][MinLength(6)][MaxLength(50)] string password,
            [Required][EmailAddress] string email)
        {
            await Messages.Dispatch(new UserCommand(userName, password, email));
            return Created(new Uri(Request.Path, UriKind.Relative), userName);
        }
    }
}
