using NBCC.Authorizaion;
using NBCC.Authorizaion.Query;
using NBCC.Courses.Commands;

namespace NBCC.WebApplicaion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        IQueryHandler<RolesQuery, IEnumerable<Role>> Messages { get; }
        public RolesController(IQueryHandler<RolesQuery, IEnumerable<Role>> messages) => Messages = messages ?? throw new ArgumentNullException(nameof(messages));

        [HttpGet]
        [Authorize]
        [DataBasedAuthorization(Messages)]

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> Get(int? roleID) => Ok(await Messages.Handle(new RolesQuery(roleID)));
    }
}
