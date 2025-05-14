using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.Infrastructure.Templating.Tokens.Formatters;

public class AgeTokenFormatter(IAgeLocalizationService localization) : IAgeTokenFormatter
{
	public string Format(Age age)
	{
		var value = string.Empty;

		if (age.Years.HasValue)
			value += $"{age.Years}{localization.GetYearShort(age.Years.Value)}";

		if (age.Months.HasValue)
			value += $" {age.Months}{localization.MonthShort}";

		if (age.Days.HasValue)
			value += $" {age.Days}{localization.DayShort}";

		return value.Trim();
	}
}