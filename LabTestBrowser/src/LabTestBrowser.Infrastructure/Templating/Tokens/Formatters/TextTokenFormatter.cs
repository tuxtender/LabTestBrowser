namespace LabTestBrowser.Infrastructure.Templating.Tokens.Formatters;

public class TextTokenFormatter : ITokenFormatter<string?>
{
	public string Format(string? value) => value ?? string.Empty;
}