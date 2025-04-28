using CommunityToolkit.Mvvm.ComponentModel;
using LabTestBrowser.UI.Notification;

namespace LabTestBrowser.UI;

public class StatusBarViewModel : ObservableObject
{
	private readonly ILogger<StatusBarViewModel> _logger;
	private NotificationViewModel? _notification;

	public StatusBarViewModel(INotificationService notificationService, ILogger<StatusBarViewModel> logger)
	{
		_logger = logger;
		notificationService.Notifications.Subscribe(Notify);
	}

	public NotificationViewModel? Notification
	{
		get => _notification;
		private set => SetProperty(ref _notification, value);
	}

	private void ResetNotification() => Notification = null;

	private async void Notify(NotificationMessage message)
	{
		try
		{
			var notification = new NotificationViewModel(message.Title, message.Level);
			Notification = notification;
			await Task.Delay(notification.TimeToLive);

			var isNotificationUpdated = Notification?.Message != message.Title;
			if (!isNotificationUpdated)
				ResetNotification();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Notification update error");
		}
	}
}