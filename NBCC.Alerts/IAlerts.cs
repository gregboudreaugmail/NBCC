namespace NBCC.Alerts;

public interface IAlerts
{
    Task Send(IEnumerable<string> emails, string body, string subject);
}