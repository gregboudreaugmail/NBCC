using System.Data;
using System.Data.SqlClient;

namespace NBCC.Authorization;

public sealed class UserService : IUserService
{
    public UserService()
    {

    }
    public bool ValidateCredentials(string username, string password)
    {
        var x = Authentiate.GenerateSaltedHash(password);

        using SqlConnection connection = new("Data Source=LAPPY\\SQLEXPRESS;Initial Catalog=CommunityCollege;Integrated Security=True;Connect Timeout=60;Encrypt=False;");
        SqlCommand command = new SqlCommand("INSERT INTO users " +
                                            "VALUES(@Username, @Password, @Hash);" +
                                            "Select SCOPE_IDENTITY();", connection);
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.Add("@Password", SqlDbType.Binary);
        command.Parameters["@Password"].Value = x.Salt;
        command.Parameters.Add("@Hash", SqlDbType.Binary);
        command.Parameters["@Hash"].Value = x.Hash;
        connection.Open();

        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.InsertCommand = command;

        int id = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
        adapter.Dispose();

        SqlCommand command1 = new SqlCommand("Select hash,password from users where username=@Username", connection);
        command1.Parameters.AddWithValue("@username", username);



        var result = command1.ExecuteReader();
        while (result.Read())
        {
            var x1 = result.GetValue(0) as byte[] ?? Array.Empty<byte>();
            var x2 = result.GetValue(1) as byte[] ?? Array.Empty<byte>();
            var x3 =  Authentiate.VerifyPassword(password, x1, x2);
            return x3;
        }
        return false;
           
    }




}
