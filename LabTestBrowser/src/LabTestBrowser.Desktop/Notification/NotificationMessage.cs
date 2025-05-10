namespace LabTestBrowser.Desktop.Notification;

public class NotificationMessage
{
	public Guid Id { get; } = Guid.NewGuid();
	public required string Title { get; init; }
	public string? Body { get; init; }
	public NotificationLevel Level { get; init; } = NotificationLevel.Info;
}