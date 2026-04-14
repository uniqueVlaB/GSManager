using MailKit.Net.Smtp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GSManager.Infrastructure.Mailer;

internal sealed partial class MailerBackgroundService(
    MailQueue queue,
    IOptions<MailerSettings> mailerSettings,
    IHostEnvironment env,
    ILogger<MailerBackgroundService> logger) : BackgroundService
{
    private readonly MailQueue _queue = queue;
    private readonly MailerSettings _settings = mailerSettings.Value;
    private readonly IHostEnvironment _env = env;

    private const int MaxConcurrency = 5;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Parallel.ForEachAsync(
            _queue.Reader.ReadAllAsync(stoppingToken),
            new ParallelOptions { MaxDegreeOfParallelism = MaxConcurrency, CancellationToken = stoppingToken },
            async (mail, ct) =>
            {
                try
                {
                    await SendAsync(mail, ct);
                }
                catch (Exception ex)
                {
                    LogEmailFailed(mail.ToEmail, ex);
                }
            });
    }

    private async Task SendAsync(MailMessage mail, CancellationToken cancellationToken)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        message.To.Add(new MailboxAddress("", mail.ToEmail));
        message.Subject = mail.Subject;
        message.Body = new TextPart("html") { Text = mail.HtmlBody };

        using var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        if (_env.IsDevelopment())
        {
            await client.ConnectAsync(_settings.Server, _settings.Port, true, cancellationToken);
        }
        else
        {
            await client.ConnectAsync(_settings.Server, cancellationToken: cancellationToken);
        }

        await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to send email to {toEmail}")]
    private partial void LogEmailFailed(string toEmail, Exception ex);
}
