using Ardalis.Result;

namespace LabTestBrowser.Desktop.Notification;

public static class ResultExtensions
{
	public static NotificationMessage ToNotification<T>(this Result<T> result, string title)
	{
		if (result.IsSuccess)
			return new NotificationMessage
			{
				Title = title,
				Level = NotificationLevel.Success
			};

		if (result.IsInvalid())
			return new NotificationMessage
			{
				Title = title,
				Body = result.ValidationErrors
					.Select(error => error.ErrorMessage)
					.Aggregate(string.Empty, (phrase, word) => $"{phrase}. {word}"),
				Level = NotificationLevel.Warning
			};

		return new NotificationMessage
		{
			Title = title,
			Body = result.Errors.Aggregate((phrase, word) => $"{phrase}. {word}"),
			Level = NotificationLevel.Error
		};
	}

	public static NotificationMessage ToNotification<T>(this Result<T> result) => ToNotification(result, string.Empty);
}