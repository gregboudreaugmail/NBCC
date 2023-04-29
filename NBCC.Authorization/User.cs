namespace NBCC.Authorization
{
    public sealed record User : IUser
    {
        public int UserId { get; }
        public string UserName { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public List<Role> Roles { get; } = new();

        public User() { }
        public User(int userId, string userName, string email, List<Role> roles)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            Roles = roles;
        }
    }

    public interface IUser
    {
        int UserId { get; }
        string UserName { get; }
        string Email { get; } 
        List<Role> Roles { get; }
    }
}
