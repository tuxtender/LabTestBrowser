using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LabTestBrowser.UI.Notification;

namespace LabTestBrowser.UI.Converters;

public class NotificationIconConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value == null)
			return DependencyProperty.UnsetValue;

		var notificationLevel = (NotificationLevel)value;

		return notificationLevel switch
		{
			NotificationLevel.Info => Application.Current.Resources["InfoIcon"],
			NotificationLevel.Warning => Application.Current.Resources["WarningIcon"],
			NotificationLevel.Error => Application.Current.Resources["ErrorIcon"],
			NotificationLevel.Success => Application.Current.Resources["SuccessIcon"],
			_ => DependencyProperty.UnsetValue
		};
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}