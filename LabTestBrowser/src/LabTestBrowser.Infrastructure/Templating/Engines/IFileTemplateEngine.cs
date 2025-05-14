namespace LabTestBrowser.Infrastructure.Templating.Engines;

public interface IFileTemplateEngine
{
	Task<MemoryStream> RenderAsync(FileStream fileStream, IReadOnlyDictionary<string, string> tokens);
}