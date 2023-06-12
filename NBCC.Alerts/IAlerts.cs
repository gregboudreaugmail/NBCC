namespace NBCC.Alerts;

public interface IAlerts
{
    Task Send(int templateId);
}