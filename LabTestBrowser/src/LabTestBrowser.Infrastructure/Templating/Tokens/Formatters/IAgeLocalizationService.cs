namespace LabTestBrowser.Infrastructure.Templating.Tokens.Formatters;

public interface IAgeLocalizationService
{
	string GetYearShort(int value);
	string MonthShort { get; }
	string DayShort { get; }
}