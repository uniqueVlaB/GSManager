namespace GSManager.Infrastructure.Mailer;

public class MailerSettings
{
    public required string Server { get; set; }
    public required short Port { get; set; }
    public required string SenderName { get; set; }
    public required string SenderEmail { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string FrontendBaseUrl { get; set; }
}
