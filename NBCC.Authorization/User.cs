namespace NBCC.Authorization;
public sealed record User(int UserId, string UserName, string Email, List<Role> Roles) : IUser;
