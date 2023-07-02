using NBCC.Authorization.Models;
using NBCC.Authorization.Query;
using NBCC.CQRS.Commands;

namespace NBCC.Authentication.WebApplication.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class RolesController : ControllerBase
{
    IQueryHandler<RolesQuery, IEnumerable<Role>> QueryHandler { get; }
    public RolesController(IQueryHandler<RolesQuery, IEnumerable<Role>> queryHandler) => QueryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));

    [HttpGet]
    [Authorize(Roles = Roles.Administrator)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<IActionResult> Get(int? roleId) => Ok(await QueryHandler.Handle(new RolesQuery(roleId)));
}
