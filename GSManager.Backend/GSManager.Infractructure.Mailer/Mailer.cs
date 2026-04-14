using Microsoft.Extensions.Options;
using GSManager.Infrastructure.Mailer.Templates;
using GSManager.Core.Abstractions.Mailer;

namespace GSManager.Infrastructure.Mailer;

internal class Mailer(IOptions<MailerSettings> mailerSettings, MailQueue queue) : IMailer
{
    private readonly MailerSettings _mailerSettings = mailerSettings.Value;
    private readonly MailQueue _queue = queue;

    public void SendEmail(string toEmail, string subject, string body)
    {
        _queue.Writer.TryWrite(new MailMessage(toEmail, subject, body));
    }

    public void SendEmailConfirmation(string toEmail, string userName, Guid userId, string token)
    {
        var encodedToken = Uri.EscapeDataString(token);
        var confirmationUrl = $"{_mailerSettings.FrontendBaseUrl}/auth/confirm-email?userId={userId}&token={encodedToken}";
        var body = EmailTemplates.EmailConfirmation(userName, confirmationUrl);
        _queue.Writer.TryWrite(new MailMessage(toEmail, "Confirm your email — GSManager", body));
    }
}
