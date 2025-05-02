using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UseCases.Export.Format;

public class AgeToken(string name, Age age) : IToken
{
	public string GetName() => name;

	public string GetValue()
	{
		var yearIdentifier = "г";
		var monthIdentifier = "м";
		var dayIdentifier = "д";

		var value = string.Empty;

		if (age.Years.HasValue)
		{
			if (age.Years > 4)
				yearIdentifier = "л";

			value += $"{age.Years}{yearIdentifier}";
		}

		if (age.Months.HasValue)
			value += $" {age.Months}{monthIdentifier}";

		if (age.Days.HasValue)
			value += $" {age.Days}{dayIdentifier}";

		return value.Trim();
	}
}