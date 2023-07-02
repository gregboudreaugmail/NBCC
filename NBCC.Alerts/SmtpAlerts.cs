namespace NBCC.Alerts
{
    public sealed class SmtpAlerts
    {
        public string FromAddress { get; init; } = string.Empty;
        public string FromDisplay { get; init; } = string.Empty;
        public string FromPassword { get; init; } = string.Empty;
    }
}
