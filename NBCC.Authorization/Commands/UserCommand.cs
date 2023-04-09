namespace NBCC.Authorizaion.Commands
{
    public class UserCommand
    {
        internal string UserName { get; }
        internal string Password { get; }
        public UserCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
