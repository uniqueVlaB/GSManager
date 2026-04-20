using GSManager.API.Filters;
using GSManager.Core.Abstractions.Mailer;
using Microsoft.AspNetCore.Mvc;

namespace GSManager.API.Controllers.Dev;

[ApiController]
[Route("api/dev/mail")]
[Tags("Dev")]
[DevOnly]
public class DevMailController(IMailer mailer) : ControllerBase
{
    private readonly IMailer _mailer = mailer;

    [HttpPost("send")]
    [EndpointSummary("Send a plain email (dev only)")]
    public IActionResult SendEmail([FromBody] DevSendEmailRequest request)
    {
        _mailer.SendEmail(request.ToEmail, request.Subject, request.Body);
        return Ok("Email event published.");
    }

    [HttpPost("send-confirmation")]
    [EndpointSummary("Send an email confirmation (dev only)")]
    public IActionResult SendConfirmation([FromBody] DevSendConfirmationRequest request)
    {
        _mailer.SendEmailConfirmation(request.ToEmail, request.UserName, request.UserId, request.Token);
        return Ok("Email confirmation event published.");
    }
}

public record DevSendEmailRequest(string ToEmail, string Subject, string Body);
public record DevSendConfirmationRequest(string ToEmail, string UserName, Guid UserId, string Token);
