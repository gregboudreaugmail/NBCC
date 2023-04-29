//using Dapper;
//using NBCC.Logging.Properties;
//using System.Data.SqlClient;
//using NBCC.Authorization;

namespace NBCC.Logging.DataAccess
{
    public sealed class AuthenticationLog : IAuthenticationLog
    {
        //Connection Connection { get; }
        //IUser User { get; }

        //public AuthenticationLog(Connection connection, IUser user)
        //{
        //    Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        //    User = user ?? throw new ArgumentNullException(nameof(user));
        //}

        //public async Task<int> Interaction()
        //{
        //    await using SqlConnection connection = new(Connection.Value);
        //    return await connection.QuerySingleAsync<int>(SqlScript.INSERT_InteractionLog, 
        //        new { User.UserId });
        //}
        public Task<int> Log()
        {
            throw new NotImplementedException();
        }
    }
}