using NBCC.Authorization;

namespace Authorization.Events;

public class UserAuthenticatedEventArgs : EventArgs
{
    public User User { get; }
    public UserAuthenticatedEventArgs(User user) => User = user;
}