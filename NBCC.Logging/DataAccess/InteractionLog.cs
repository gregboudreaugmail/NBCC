using Dapper;
using NBCC.Logging.Models;
using NBCC.Logging.Properties;
using System.Data.SqlClient;

namespace NBCC.Logging.DataAccess
{
    public sealed class InteractionLog : IInteractionLog
    {
        Connection Connection { get; }
        public InteractionLog(Connection connection) =>
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        
        public async Task Log(Interaction? interaction)
        {
            await using SqlConnection connection = new(Connection.Value);
            await connection.ExecuteAsync(SqlScript.INSERT_Interaction, interaction);
        }
    }
}
