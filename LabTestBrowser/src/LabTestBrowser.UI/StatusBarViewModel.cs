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
			Notification = new NotificationViewModel(message);
			await Task.Delay(Notification.TimeToLive);

			var isNotificationUpdated = Notification?.Id != message.Id;
			if (!isNotificationUpdated)
				ResetNotification();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Notification update error");
		}
	}
}