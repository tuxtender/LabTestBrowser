namespace LabTestBrowser.Infrastructure.Export;

public interface ITextTemplateEngine
{
	string Render(string template, Dictionary<string, string> tokens);
}