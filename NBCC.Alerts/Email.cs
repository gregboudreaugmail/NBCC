using System.Net;
using System.Net.Mail;

namespace NBCC.Alerts;

public class Email : IAlerts
{
    /*
     * Note 24
     * Using the 3rd party
     * you'll want to keep your 3rd parties isolated like this on the condition that it becomes
     * obsolete or stops working for whatever reason.  You don't want to muck around in your
     * application's code when that happens as it'll make more chances for bugs.
     */
    SmtpAlerts SmtpAlerts { get; }

    public Email(SmtpAlerts smtpAlerts) => SmtpAlerts = smtpAlerts ?? throw new ArgumentNullException(nameof(smtpAlerts));

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