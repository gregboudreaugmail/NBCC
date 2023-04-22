namespace NBCC.Authorization
{
    public sealed record User
    {
        public int UserId { get; }
        public string UserName { get; } = string.Empty;
        public List<Role> Roles { get; } = new();

        public User() { }
        public User(int userId, string userName, List<Role> roles)
        {
            UserId = userId;
            UserName = userName;
            Roles = roles;
        }
    }
}
