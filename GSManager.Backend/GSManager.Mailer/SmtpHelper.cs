using MailKit.Net.Smtp;
using MimeKit;

namespace GSManager.Mailer;

internal static class SmtpHelper
{
    public static async Task SendAsync(
        string toEmail,
        string subject,
        string htmlBody,
        MailerSettings settings,
        bool isDevelopment,
        CancellationToken cancellationToken)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(settings.SenderName, settings.SenderEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        if (isDevelopment)
        {
            await client.ConnectAsync(settings.Server, settings.Port, true, cancellationToken);
        }
        else
        {
            await client.ConnectAsync(settings.Server, cancellationToken: cancellationToken);
        }

        await client.AuthenticateAsync(settings.Username, settings.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}
