namespace NBCC.Authorizaion.Commands
{
    public sealed class UserCommand
    {
        internal string UserName { get; }
        internal string Password { get; }
        internal string Email { get; }

        public UserCommand(string userName, string password, string email)
        {
            UserName = userName;
            Password = password;
            Email = email;
        }
    }
}
