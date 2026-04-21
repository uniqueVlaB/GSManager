using GSManager.Contracts.Events.Mail;
using GSManager.Mailer.Templates;
using MassTransit;
using Microsoft.Extensions.Options;

namespace GSManager.Mailer.Consumers;

public sealed class EmailConfirmationConsumer(
    IOptions<MailerSettings> settings,
    IHostEnvironment env,
    ILogger<EmailConfirmationConsumer> logger) : IConsumer<EmailConfirmationRequestedEvent>
{
    public async Task Consume(ConsumeContext<EmailConfirmationRequestedEvent> context)
    {
        var msg = context.Message;
        logger.LogInformation("Sending confirmation email to {Email}", msg.ToEmail);

        var encodedToken = Uri.EscapeDataString(msg.Token);
        var confirmationUrl = $"{settings.Value.FrontendBaseUrl}/auth/confirm-email?userId={msg.UserId}&token={encodedToken}";
        var body = EmailTemplates.EmailConfirmation(msg.UserName, confirmationUrl);

        await SmtpHelper.SendAsync(
            msg.ToEmail,
            "Confirm your email — GSManager",
            body,
            settings.Value,
            env.IsDevelopment(),
            context.CancellationToken);

        logger.LogInformation("Confirmation email sent successfully to {Email}", msg.ToEmail);
    }
}
