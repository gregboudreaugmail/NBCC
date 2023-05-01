namespace Authorization.Events;

public class Authenticated
{
    public virtual void OnUserAuthenticated(UserAuthenticatedEventArgs e) => UserAuthenticated?.Invoke(this, e);
    public delegate void UserAuthenticatedEventHandler(object sender, UserAuthenticatedEventArgs e);
    public static event UserAuthenticatedEventHandler? UserAuthenticated;
}