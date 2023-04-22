using Dapper;
using NBCC.Authorization.Properties;
using System.Data.SqlClient;

namespace NBCC.Authorization.DataAccess;

public sealed class AuthenticationRepository : IAuthenticationRepository
{
    AuthenticationConnection Connection { get; }

    public AuthenticationRepository(AuthenticationConnection connection) =>
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));

    public async Task<User?> GetUser(string userName)
    {
        var userDictionary = new Dictionary<int, User>();
        await using SqlConnection connection = new(Connection.Value);
        await connection.QueryAsync<User, Role, User>(SqlScript.SELECT_userByUserName,
            map: (user, role) =>
            {
                if (!userDictionary.TryGetValue(user.UserId, out var u))
                {
                    u = user;
                    u.Roles.Add(role);
                    userDictionary.Add(u.UserId, u);
                }

                u.Roles.Add(role);
                return u;
            }, splitOn: "RoleId", param: new { userName });
        return userDictionary.FirstOrDefault().Value;
    }

    public async Task<bool> AuthenticateUser(string userName, string password)
    {
        await using SqlConnection connection = new(Connection.Value);
        var persistedCredentials = await connection.QueryFirstOrDefaultAsync<HashSalt>(SqlScript.SELECT_passwordByUserName, new { userName });
        return (persistedCredentials == null || Authenticate.VerifyPassword(password, persistedCredentials.Hash, persistedCredentials.Password));

    }
}