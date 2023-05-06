using NBCC.Authorization.Properties;
namespace NBCC.Authorization.DataAccess;

public sealed class RolesRepository : IRolesRepository
    {
        Connection Connection { get; }

        public RolesRepository(Connection connection) =>
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));

        public async Task<IEnumerable<Role>> GetRoles(int? roleId)
        {
            await using SqlConnection connection = new(Connection.Value);
            return await connection.QueryAsync<Role>(SqlScript.SELECT_Roles, new { roleID = roleId });
        }
    }
