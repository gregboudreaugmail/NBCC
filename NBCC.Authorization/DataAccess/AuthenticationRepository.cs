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

    public async Task<User?> GetUser(string userName)
    {
        var userDictionary = new Dictionary<int, User>();
        await using SqlConnection connection = new(Connection.Value);
        await connection.QueryAsync<User, Role, User>(SqlScript.SELECT_userByUserName,
            map: (user, role) =>
            {
                if (!userDictionary.TryGetValue(user.UserID, out var u))
                {
                    u = user;
                    u.Roles.Add(role);
                    userDictionary.Add(u.UserID, u);
                }

                u.Roles.Add(role);
                return u;
            }, splitOn: "Roleid", param: new { userName });
        return userDictionary.FirstOrDefault().Value;
    }

    public async Task<bool> AuthenticateUser(string userName, string password)
    {
        await using SqlConnection connection = new(Connection.Value);
        var persistedCredentials = await connection.QueryFirstOrDefaultAsync<HashSalt>(SqlScript.SELECT_passwordByUserName, new { userName });
        return (persistedCredentials == null || Authentiate.VerifyPassword(password, persistedCredentials.Hash, persistedCredentials.Password));

    }
}