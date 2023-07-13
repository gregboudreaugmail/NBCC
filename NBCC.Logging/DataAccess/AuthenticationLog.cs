using NBCC.Logging.Properties;
namespace NBCC.Logging.DataAccess;

/*
 * Note 28
 * Auditing
 * Having a trail of what happens in your app when it goes live is crucial.  I have setup
 * three types of auditing.  Authentication, to know how logged in, exceptions to capture
 * errors and interactions to know how the user is navigating the app
 */
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
