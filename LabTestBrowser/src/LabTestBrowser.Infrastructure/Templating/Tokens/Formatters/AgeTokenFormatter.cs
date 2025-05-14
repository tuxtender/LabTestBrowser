using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.Infrastructure.Templating.Tokens.Formatters;

public class AgeTokenFormatter : IAgeTokenFormatter
{
	public string Format(Age age)
	{
		var value = string.Empty;
		var yearIdentifier = age.Years > 4 ? "л" : "г";
		var monthIdentifier = "м";
		var dayIdentifier = "д";

		if (age.Years.HasValue)
			value += $"{age.Years}{yearIdentifier}";

		if (age.Months.HasValue)
			value += $" {age.Months}{monthIdentifier}";

		if (age.Days.HasValue)
			value += $" {age.Days}{dayIdentifier}";

		return value.Trim();
	}
}