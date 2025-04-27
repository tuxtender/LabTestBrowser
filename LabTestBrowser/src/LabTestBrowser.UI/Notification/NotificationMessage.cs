namespace LabTestBrowser.UI.Notification;

public class NotificationMessage
{
	public required string Title { get; init; }
	public required string Body { get; init; }
	public NotificationLevel Level { get; init; } = NotificationLevel.Info;
}