namespace LabTestBrowser.Infrastructure.Export;

public interface ITextTemplateEngine
{
	string Render(string template, IReadOnlyDictionary<string, string> tokens);
}