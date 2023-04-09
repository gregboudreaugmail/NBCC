namespace NBCC.Authorizaion.DataAccess
{
    public interface IRolesRepository
    {
        Task<IEnumerable<Role>> GetRoles(int? roleID);
    }
  
}
