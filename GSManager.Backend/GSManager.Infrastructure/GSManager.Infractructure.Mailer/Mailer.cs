using GSManager.Contracts.Events.Mail;
using GSManager.Core.Abstractions.Mailer;
using MassTransit;

namespace GSManager.Infrastructure.Mailer;

internal class Mailer(IPublishEndpoint publishEndpoint) : IMailer
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public void SendEmail(string toEmail, string subject, string body)
    {
        _ = _publishEndpoint.Publish(new SendEmailRequestedEvent(toEmail, subject, body));
    }

    public void SendEmailConfirmation(string toEmail, string userName, Guid userId, string token)
    {
        _ = _publishEndpoint.Publish(new EmailConfirmationRequestedEvent(toEmail, userName, userId, token));
    }
}
