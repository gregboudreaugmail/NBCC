using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBCC.Authorization.Query;
using NBCC.Authorization;
using NBCC.Courses.Commands;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace NBCC.WebApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public sealed class RolesController : ControllerBase
    {
        IQueryHandler<RolesQuery, IEnumerable<Role>> Messages { get; }
        public RolesController(IQueryHandler<RolesQuery, IEnumerable<Role>> messages) => Messages = messages ?? throw new ArgumentNullException(nameof(messages));

        [HttpGet]
        [Authorize(Roles = Roles.Administrator)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> Get(
            [Display(Name = "Role ID")]
            [Range(0, 2147483647)]
                int? roleId) => Ok(await Messages.Handle(new RolesQuery(roleId)));
    }
}
