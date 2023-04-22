using System.Data.SqlClient;
using System.Text;

namespace NBCC.WebApplication;

public static class SqlExceptionHandler
{
    public static async Task LogSqlExceptionAsync(Exception ex)
    {
        if (ex is SqlException exception)
        {
            StringBuilder errorMessages = new();

            for (var i = 0; i < exception.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + "\n" +
                    "Message: " + exception.Errors[i].Message + "\n" +
                    "Error Number: " + exception.Errors[i].Number + "\n" +
                    "LineNumber: " + exception.Errors[i].LineNumber + "\n" +
                    "Source: " + exception.Errors[i].Source + "\n" +
                    "Procedure: " + exception.Errors[i].Procedure + "\n");
            }
            //await logging.Log(errorMessages.ToString());
        }
    }
}