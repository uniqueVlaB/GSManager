namespace GSManager.Core.Abstractions.Mailer;

public interface IMailer
{
    void SendEmail(string toEmail, string subject, string body);
    void SendEmailConfirmation(string toEmail, string userName, Guid userId, string token);
}
