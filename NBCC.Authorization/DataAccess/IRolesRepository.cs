using NBCC.Authorization.Models;

namespace NBCC.Authorization.DataAccess;

public interface IRolesRepository
{
    Task<IEnumerable<Role>> GetRoles(int? roleId);
}

