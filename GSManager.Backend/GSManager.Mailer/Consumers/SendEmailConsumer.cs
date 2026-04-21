using GSManager.Contracts.Events.Mail;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GSManager.Mailer.Consumers;

public sealed class SendEmailConsumer(
    IOptions<MailerSettings> settings,
    IHostEnvironment env,
    ILogger<SendEmailConsumer> logger) : IConsumer<SendEmailRequestedEvent>
{
    public async Task Consume(ConsumeContext<SendEmailRequestedEvent> context)
    {
        var msg = context.Message;
        logger.LogInformation("Sending email to {Email}, subject: {Subject}", msg.ToEmail, msg.Subject);

        await SmtpHelper.SendAsync(
            msg.ToEmail,
            msg.Subject,
            msg.HtmlBody,
            settings.Value,
            env.IsDevelopment(),
            context.CancellationToken);

        logger.LogInformation("Email sent successfully to {Email}", msg.ToEmail);
    }
}
