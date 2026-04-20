namespace GSManager.Contracts.Events.Mail;

public record SendEmailRequestedEvent(
    string ToEmail,
    string Subject,
    string HtmlBody);
