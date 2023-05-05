using Dapper;
using NBCC.Logging.Properties;
using System.Data.SqlClient;

namespace NBCC.Logging.DataAccess
{
    public sealed class AuthenticationLog : IAuthenticationLog
    {
        Connection Connection { get; }

        public AuthenticationLog(Connection connection) =>
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));

        public async Task<int> Log(int userId)
        {
            try
            {
                await using SqlConnection connection = new(Connection.Value);
                return await connection.QuerySingleAsync<int>(SqlScript.INSERT_AuthenticationLog, new { userId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }
    }
}