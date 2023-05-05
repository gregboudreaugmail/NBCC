using Dapper;
using NBCC.Logging.Models;
using NBCC.Logging.Properties;
using System.Data.SqlClient;

namespace NBCC.Logging.DataAccess;

public sealed class ExceptionLog : IExceptionLog
{
    Connection Connection { get; }
    IAuthenticationSession AuthenticationSession { get; }

    public ExceptionLog(Connection connection, IAuthenticationSession authenticationSession)
    {
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        AuthenticationSession = authenticationSession ?? throw new ArgumentNullException(nameof(authenticationSession));
    }
    public async Task<int> Log(Exception ex)
    {
        try
        {
            await using SqlConnection connection = new(Connection.Value);
            var exception = ex.ToString();
            return await connection.QueryFirstAsync<int>(SqlScript.INSERT_ExceptionLog,
            new
            {
                AuthenticationSession.AuthenticationId,
                exception
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}