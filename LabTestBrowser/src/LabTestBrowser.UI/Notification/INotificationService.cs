namespace LabTestBrowser.UI.Notification;

public interface INotificationService
{
	Task PublishAsync(NotificationMessage notification);
	IAsyncEnumerable<NotificationMessage> ListenAsync(CancellationToken cancellationToken = default);
}