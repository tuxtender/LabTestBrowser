namespace LabTestBrowser.Infrastructure.Templating.Tokens;

public class TemplateToken<T>(
	string name,
	T rawValue,
	ITokenFormatter<T> formatter) : ITemplateToken<T>
{
	public string Name { get; } = name;
	public T RawValue { get; } = rawValue;
	public string FormattedValue => formatter.Format(RawValue);
}