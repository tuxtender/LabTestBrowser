using System.Globalization;
using System.Windows.Data;
using ModernWpf;

namespace LabTestBrowser.Desktop.Converters;

public class ApplicationThemeConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value == null)
			return ElementTheme.Default;

		var mode = (ApplicationMode)value;

		return mode switch
		{
			ApplicationMode.Light => ElementTheme.Light,
			ApplicationMode.Dark => ElementTheme.Dark,
			_ => ElementTheme.Default
		};
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}