using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LabTestBrowser.Desktop.Notification;

namespace LabTestBrowser.Desktop.Converters;

public class NotificationIconConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value == null)
			return DependencyProperty.UnsetValue;

		var notificationLevel = (NotificationLevel)value;

		return notificationLevel switch
		{
			NotificationLevel.Info => Application.Current.Resources["InfoPath"],
			NotificationLevel.Warning => Application.Current.Resources["WarningPath"],
			NotificationLevel.Error => Application.Current.Resources["ErrorPath"],
			NotificationLevel.Success => Application.Current.Resources["SuccessPath"],
			_ => DependencyProperty.UnsetValue
		};
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}