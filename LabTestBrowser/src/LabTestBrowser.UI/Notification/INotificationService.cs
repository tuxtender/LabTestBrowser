namespace LabTestBrowser.UI.Notification;

public interface INotificationService
{
	Task PublishAsync(NotificationMessage notification);
	IObservable<NotificationMessage> Notifications { get; }
}