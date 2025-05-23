namespace LabTestBrowser.Desktop.Notification;

public class NotificationViewModel(NotificationMessage notification)
{
	public Guid Id { get; } = notification.Id;

	public string Message { get; } = $"{notification.Title}. {notification.Body?.Trim('.')}".Trim(' ', '.');

	public NotificationLevel Level { get; } = notification.Level;

	public TimeSpan TimeToLive { get; } = notification.Level switch
	{
		NotificationLevel.Info => TimeSpan.FromSeconds(2),
		NotificationLevel.Success => TimeSpan.FromSeconds(2),
		NotificationLevel.Warning or NotificationLevel.Error => TimeSpan.FromSeconds(15),
		_ => TimeSpan.Zero
	};
}