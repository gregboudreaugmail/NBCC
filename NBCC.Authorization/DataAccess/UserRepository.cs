using NBCC.Authorization.Properties;
namespace NBCC.Authorization.DataAccess;

public sealed class UserRepository : IUserRepository
    {
        Connection Connection { get; }

        public UserRepository(Connection connection) =>
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));

        public async Task Create(string userName, string password, string email)
        {
            await using SqlConnection connection = new(Connection.Value);
            var credentials = Hashing.GenerateSaltedHash(password);
            await connection.ExecuteAsync(SqlScript.INSERT_User, new
            {
                userName,
                email,
                credentials.Password,
                credentials.Hash
            });
        }
    }
