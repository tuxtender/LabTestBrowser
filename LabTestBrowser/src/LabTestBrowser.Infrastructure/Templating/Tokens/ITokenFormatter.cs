namespace LabTestBrowser.Infrastructure.Templating.Tokens;

public interface ITokenFormatter<T>
{
	string Format(T value);
}