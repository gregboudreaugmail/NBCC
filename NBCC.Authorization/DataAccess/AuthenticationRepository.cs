using Dapper;
using NBCC.Authorizaion.Properties;
using NBCC.Authorization;
using System.Data.SqlClient;

namespace NBCC.Authorizaion.DataAccess;

public sealed class AuthenticationRepository : IAuthenticationRepository
{
    AuthenticationConnection Connection { get; }

    public AuthenticationRepository(AuthenticationConnection connection) =>
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));

    public async Task<bool> ValidateCredentials(string userName, string password)
    {
        await using SqlConnection connection = new(Connection.Value);
        var stored = await connection.QueryFirstAsync<HashSalt>(SqlScript.SELECT_passwordByUserName, new { userName });
        return Authentiate.VerifyPassword(password, stored.Hash, stored.Password);
    }
}