namespace NBCC.Courses.DataAccess;

public sealed class Connection
{
    public string Value { get; }
    public Connection(string value) => Value = value;
}