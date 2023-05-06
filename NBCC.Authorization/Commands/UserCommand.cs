namespace NBCC.Authorization.Commands;

public sealed record UserCommand(string UserName, string Password, string Email);
