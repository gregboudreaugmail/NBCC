using System.Net;
using System.Net.Mail;

namespace NBCC.Alerts;

public class Email : IAlerts
{
    SmtpAlerts SmtpAlerts { get; }

    public Email(SmtpAlerts smtpAlerts) => SmtpAlerts = smtpAlerts;

    public async Task Send(IEnumerable<string> emails, string body, string subject)
    {
        var fromAddress = new MailAddress(SmtpAlerts.FromAddress, SmtpAlerts.FromDisplay);
        var recipients = emails as string[] ?? emails.ToArray();

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, SmtpAlerts.FromPassword)
        };

        using var message = new MailMessage
        {
            Subject = subject,
            Body = body,
            From = fromAddress
        };

        foreach (var email in recipients)
            message.To.Add(email);

        await smtp.SendMailAsync(message);
    }
}