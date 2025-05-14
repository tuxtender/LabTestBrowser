namespace LabTestBrowser.Infrastructure.Templating.Engines;

public interface ITextTemplateEngine
{
	string Render(string template, IReadOnlyDictionary<string, string> tokens);
}