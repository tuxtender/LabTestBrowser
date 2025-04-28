using LabTestBrowser.UI.Notification;

namespace LabTestBrowser.UI;

public class NotificationViewModel(string message, NotificationLevel level)
{
	public string Message { get; init; } = message;
	public NotificationLevel Level { get; init; } = level;

	public TimeSpan TimeToLive { get; init; } = level switch
	{
		NotificationLevel.Success or NotificationLevel.Info => TimeSpan.FromSeconds(5),
		NotificationLevel.Warning or NotificationLevel.Error => TimeSpan.FromSeconds(15),
		_ => TimeSpan.Zero
	};
}