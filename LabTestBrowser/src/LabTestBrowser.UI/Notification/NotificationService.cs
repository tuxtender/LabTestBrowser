using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace LabTestBrowser.UI.Notification;

public class NotificationService : INotificationService
{
	private readonly Channel<NotificationMessage> _channel = Channel.CreateUnbounded<NotificationMessage>();

	public async Task PublishAsync(NotificationMessage message)
	{
		await _channel.Writer.WriteAsync(message);
	}

	public async IAsyncEnumerable<NotificationMessage> ListenAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		while (await _channel.Reader.WaitToReadAsync(cancellationToken))
		{
			while (_channel.Reader.TryRead(out var message))
			{
				yield return message;
			}
		}
	}
}