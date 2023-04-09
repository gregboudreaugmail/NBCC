﻿using Dapper;
using NBCC.Authorizaion.Properties;
using NBCC.Authorization;
using System.Data.SqlClient;

namespace NBCC.Authorizaion.DataAccess
{
    public sealed class UserRepository : IUserRepository
    {
        AuthenticationConnection Connection { get; }

        public UserRepository(AuthenticationConnection connection) =>
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));

        public async Task Create(string userName, string password)
        {
            await using SqlConnection connection = new(Connection.Value);
            var credentials = Authentiate.GenerateSaltedHash(password);
            await connection.ExecuteAsync(SqlScript.INSERT_User, new { userName, credentials.Password, credentials.Hash });
        }
    }
}