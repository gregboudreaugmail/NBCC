namespace NBCC.Authorization.DataAccess;

public interface IRolesRepository
{
    Task<IEnumerable<Role>> GetRoles(int? roleId);
}

