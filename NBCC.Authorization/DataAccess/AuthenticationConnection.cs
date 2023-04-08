namespace NBCC.Authorizaion.DataAccess;

public sealed class AuthenticationConnection
{
    public string Value { get; }
    public AuthenticationConnection(string value) => Value = value;
}