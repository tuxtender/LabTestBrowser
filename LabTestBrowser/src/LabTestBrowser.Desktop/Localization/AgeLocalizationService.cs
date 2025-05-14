using LabTestBrowser.Infrastructure.Templating.Tokens.Formatters;

namespace LabTestBrowser.Desktop.Localization;

public class AgeLocalizationService : IAgeLocalizationService
{
	public string GetYearShort(int value)
	{
		return value > 4 ? UI.Resources.Strings.Years_Plural_Short : UI.Resources.Strings.Years_Singular_Short;
	}

	public string MonthShort => UI.Resources.Strings.Month_Short;
	public string DayShort => UI.Resources.Strings.Day_Short;
}