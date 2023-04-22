﻿using Dapper;
using NBCC.Authorization;
using System.Data.SqlClient;
using NBCC.Authorization.Properties;

namespace NBCC.Authorization.DataAccess
{
    public sealed class UserRepository : IUserRepository
    {
        AuthenticationConnection Connection { get; }

        public UserRepository(AuthenticationConnection connection) =>
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));

        public async Task Create(string userName, string password, string email)
        {
            await using SqlConnection connection = new(Connection.Value);
            var credentials = Authenticate.GenerateSaltedHash(password);
            await connection.ExecuteAsync(SqlScript.INSERT_User, new
            {
                userName,
                email,
                credentials.Password,
                credentials.Hash
            });
        }
    }
}
