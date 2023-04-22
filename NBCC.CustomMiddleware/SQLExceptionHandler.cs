using System.Data.SqlClient;
using System.Text;

namespace NBCC.WebApplicaion;

public static class SQLExceptionHandler
{
    public static async Task LogSQLExceptionAsync(Exception ex)
    {
        if (ex is SqlException exception)
        {
            StringBuilder errorMessages = new();

            for (int i = 0; i < exception.Errors.Count; i++)
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