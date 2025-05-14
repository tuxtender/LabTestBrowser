namespace LabTestBrowser.Infrastructure.Templating.Tokens.Formatters;

public class PersonNameTokenFormatter : IPersonNameTokenFormatter
{
	public string Format(string? name) => name?.Split().First() ?? string.Empty;
}