namespace LabTestBrowser.Infrastructure.Templating.Tokens.Formatters;

public class DateTokenFormatter : ITokenFormatter<DateOnly>
{
	public string Format(DateOnly date) => date.ToString("dd.MM.yyyy");
}