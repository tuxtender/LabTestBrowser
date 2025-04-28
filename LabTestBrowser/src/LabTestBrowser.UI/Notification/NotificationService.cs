using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace LabTestBrowser.UI.Notification;

public class NotificationService : INotificationService, IDisposable
{
	private readonly ISubject<NotificationMessage> _subject = Subject.Synchronize(new ReplaySubject<NotificationMessage>(1));

	public async Task PublishAsync(NotificationMessage message)
	{
		_subject.OnNext(message);
		await Task.CompletedTask;
	}

	public IObservable<NotificationMessage> Notifications => _subject.AsObservable();

	public void Dispose()
	{
		_subject.OnCompleted();
		(_subject as IDisposable)?.Dispose();
	}
}