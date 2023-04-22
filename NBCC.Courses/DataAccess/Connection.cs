namespace NBCC.Courses.DataAccess;

public sealed record Connection
{
    public string Value { get; }
    public Connection(string value) => Value = value;
}