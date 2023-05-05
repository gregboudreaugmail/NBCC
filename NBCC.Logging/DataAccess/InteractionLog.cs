using Dapper;
using NBCC.Logging.Models;
using NBCC.Logging.Properties;
using System.Data.SqlClient;

namespace NBCC.Logging.DataAccess
{
    public sealed class InteractionLog : IInteractionLog
    {
        Connection Connection { get; }
        IAuthenticationSession AuthenticationSession { get; }

        public InteractionLog(Connection connection, IAuthenticationSession authenticationSession)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            AuthenticationSession = authenticationSession ?? throw new ArgumentNullException(nameof(authenticationSession));
        }

        public async Task Log(Interaction interaction)
        {
            await using SqlConnection connection = new(Connection.Value);
            await connection.ExecuteAsync(SqlScript.INSERT_InteractionLog,
                new
                {
                    AuthenticationSession.AuthenticationId,
                    interaction.AssemblyName,
                    interaction.Command,
                    interaction.Parameters
                });
        }
    }
}
