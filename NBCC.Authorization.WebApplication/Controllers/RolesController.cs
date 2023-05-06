using NBCC.Authorization;
using NBCC.Authorization.Query;
using NBCC.Courses.Commands;

namespace NBCC.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class RolesController : ControllerBase
{
    IQueryHandler<RolesQuery, IEnumerable<Role>> Dispatcher { get; }
    public RolesController(IQueryHandler<RolesQuery, IEnumerable<Role>> dispatcher) => Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));

    [HttpGet]
    [Authorize(Roles = Roles.Administrator)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> Get(int? roleId) => Ok(await Dispatcher.Handle(new RolesQuery(roleId)));
}
