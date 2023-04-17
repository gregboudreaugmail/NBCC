namespace NBCC.Authorizaion
{
    public sealed class User
    {
        public int UserID { get; }
        public string UserName { get; } = string.Empty;
        public List<Role> Roles { get; } = new List<Role>();

        public User() { }
        public User(int userID, string userName, List<Role> roles)
        {
            UserID = userID;
            UserName = userName;
            Roles = roles;
        }
    }
}
