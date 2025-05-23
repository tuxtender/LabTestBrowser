using System.Globalization;
using System.Windows.Data;

namespace LabTestBrowser.Desktop.Converters;

public class AgeConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if(value is null)
			return double.NaN;
		
		if(double.TryParse(value.ToString(), out var age))
			return age;
		
		return double.NaN;
	}

	public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is double.NaN)
			return null;

		return value?.ToString();
	}
}