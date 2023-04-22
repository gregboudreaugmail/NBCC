namespace NBCC.Authorizaion.DataAccess;

public sealed record AuthenticationConnection
{
    public string Value { get; }
    public AuthenticationConnection(string value) => Value = value;
}