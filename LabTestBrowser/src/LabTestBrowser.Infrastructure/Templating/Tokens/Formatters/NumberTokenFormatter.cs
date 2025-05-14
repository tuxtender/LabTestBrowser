namespace LabTestBrowser.Infrastructure.Templating.Tokens.Formatters;

public class NumberTokenFormatter : ITokenFormatter<int?>
{
	public string Format(int? number) => number.HasValue ? number.Value.ToString() : string.Empty;
}