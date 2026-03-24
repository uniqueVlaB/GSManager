using System.Threading.Channels;

namespace GSManager.Infrastructure.Mailer;

internal sealed class MailQueue
{
    private readonly Channel<MailMessage> _channel = Channel.CreateUnbounded<MailMessage>(
        new UnboundedChannelOptions { SingleReader = false });

    public ChannelWriter<MailMessage> Writer => _channel.Writer;
    public ChannelReader<MailMessage> Reader => _channel.Reader;
}
