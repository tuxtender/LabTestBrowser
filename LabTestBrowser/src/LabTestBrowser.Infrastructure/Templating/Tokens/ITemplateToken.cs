namespace LabTestBrowser.Infrastructure.Templating.Tokens;

public interface ITemplateToken
{
	string Name { get; }
	string FormattedValue { get; }
}

/// <summary>
/// Ensures type safety when using <see cref="ITokenFormatter"/> 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ITemplateToken<T> : ITemplateToken
{
	T RawValue { get; }
}