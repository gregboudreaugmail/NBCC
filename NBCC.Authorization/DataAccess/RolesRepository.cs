using Dapper;
using NBCC.Authorizaion.Properties;
using System.Data.SqlClient;

namespace NBCC.Authorizaion.DataAccess
{
    public sealed class RolesRepository : IRolesRepository
    {
        AuthenticationConnection Connection { get; }

        public RolesRepository(AuthenticationConnection connection) =>
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));

        public async Task<IEnumerable<Role>> GetRoles(int? roleID)
        {
            await using SqlConnection connection = new(Connection.Value);
            return await connection.QueryAsync<Role>(SqlScript.SELECT_Roles, new { roleID });
        }
    }
}
