namespace NBCC.WebApplication;

public static class SqlExceptionHandler
{
    public static async Task<int?> LogSqlException(Exception ex, ILoggerAsync logger)
    {
        if (ex is not SqlException exception) return null;
        StringBuilder errorMessages = new();

        for (var i = 0; i < exception.Errors.Count; i++)
            errorMessages.Append("Index #" + i + "\n" +
                                 "Message: " + exception.Errors[i].Message + "\n" +
                                 "Error Number: " + exception.Errors[i].Number + "\n" +
                                 "LineNumber: " + exception.Errors[i].LineNumber + "\n" +
                                 "Source: " + exception.Errors[i].Source + "\n" +
                                 "Procedure: " + exception.Errors[i].Procedure + "\n");
        return await logger.Log(new Exception(errorMessages.ToString()));
    }
}