namespace GSManager.Infrastructure.Mailer;

internal record MailMessage(string ToEmail, string Subject, string HtmlBody);
