namespace GSManager.Contracts.Events.Mail;

public record EmailConfirmationRequestedEvent(
    string ToEmail,
    string UserName,
    Guid UserId,
    string Token);
