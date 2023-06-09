﻿using NBCC.Authorization.Models;
using NBCC.Authorization.Properties;
namespace NBCC.Authorization.DataAccess;

public sealed class AuthenticationRepository : IAuthenticationRepository
{
    Connection Connection { get; }

    public AuthenticationRepository(Connection connection) =>
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));

    public async Task<User?> Get(string userName)
    {
        var users = new Dictionary<int, User>();
        await using SqlConnection connection = new(Connection.Value);
        await connection.QueryAsync<User, Role, User>(SqlScript.SELECT_userByUserName,
            map: (user, role) =>
            {
                if (!users.TryGetValue(user.UserId, out var u))
                {
                    u = user;
                    u.Roles.Add(role);
                    users.Add(u.UserId, u);
                }

                u.Roles.Add(role);
                return u;
            }, splitOn: "RoleId", param: new { userName });
        return users.FirstOrDefault().Value;
    }

    public async Task<bool> AuthenticateUser(string userName, string password)
    {
        await using SqlConnection connection = new(Connection.Value);
        var persistedCredentials = await connection.QueryFirstOrDefaultAsync<HashSalt>(SqlScript.SELECT_passwordByUserName, new { userName });
        return persistedCredentials != null && Hashing.VerifyPassword(password, persistedCredentials.Hash, persistedCredentials.Password);
    }
}