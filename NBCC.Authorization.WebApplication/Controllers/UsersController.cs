using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBCC.Authorizaion.Commands;
using NBCC.Authorization;
using NBCC.Courses.Commands;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace NBCC.WebApplicaion.Controllers
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
        public async Task<IActionResult> Post(
            [Required(ErrorMessage = "The User Name is required")]
            [Display(Name ="User Name")]
            [MaxLength(50)]
                string userName,

            [Required(ErrorMessage = "The password is required")]
            [Display(Name = "Password")]
            [MinLength(6,ErrorMessage = "Password must be at least six characters long")]
            [MaxLength(50, ErrorMessage = "Password must not exceed fifty characters long")]
                string password,

            [Required(ErrorMessage = "The Email address is required")]
            [Display(Name = "Email address")]
            [MaxLength(320, ErrorMessage = "Email address must not exceed three hundred twenty characters long")]
            [EmailAddress(ErrorMessage = "Invalid Email address")]
               string email
            )
        {
            await Messages.Dispatch(new UserCommand(userName, password, email));
            return Created(new Uri(Request.Path, UriKind.Relative), userName);
        }
    }
}
