using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Text;

namespace NBCC.WebApplication;

public static class SqlExceptionHandler
{
    public static void LogSqlException(Exception ex, ILogger<CustomMiddleware> logger)
    {
        if (ex is not SqlException exception) return;
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
        logger.LogInformation("{error}",errorMessages.ToString());
    }
}