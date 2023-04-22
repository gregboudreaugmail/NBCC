namespace NBCC.Logs.DataAccess;

public sealed class LoggingConnection
{
    public string Value { get; }
    public LoggingConnection(string value) => Value = value;
}