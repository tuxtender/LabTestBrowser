﻿using System.Globalization;
using System.Windows.Data;

namespace LabTestBrowser.Desktop.Converters;

public class DateConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return ((DateOnly)value).ToDateTime(TimeOnly.MinValue);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return DateOnly.FromDateTime((DateTime)value);
	}
}