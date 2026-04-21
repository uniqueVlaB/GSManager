using System.ComponentModel.DataAnnotations;

namespace GSManager.Mailer;

public class MailerSettings
{
    [Required] public required string Server { get; set; }
    [Required] public required short Port { get; set; }
    [Required] public required string SenderName { get; set; }
    [Required] public required string SenderEmail { get; set; }
    [Required] public required string Username { get; set; }
    [Required] public required string Password { get; set; }
    [Required] public required string FrontendBaseUrl { get; set; }
}
