using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LabTestBrowser.Desktop.Notification;

public partial class StatusBarViewModel : ObservableObject
{
	private readonly ILogger<StatusBarViewModel> _logger;
	private NotificationViewModel? _notification;
	private ApplicationMode _applicationMode = ApplicationMode.Light;

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

	public ApplicationMode ApplicationMode
	{
		get => _applicationMode;
		private set => SetProperty(ref _applicationMode, value);
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

	[RelayCommand]
	private void ToggleTheme()
	{
		ApplicationMode = ApplicationMode != ApplicationMode.Light ? ApplicationMode.Light : ApplicationMode.Dark;
	}
}