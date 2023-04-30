namespace NBCC.Authorization;

public interface IUser
{
    int UserId { get; }
    string UserName { get; }
    string Email { get; } 
    List<Role> Roles { get; }
}