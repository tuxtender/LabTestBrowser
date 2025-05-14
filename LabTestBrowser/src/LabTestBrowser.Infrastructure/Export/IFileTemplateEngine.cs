namespace LabTestBrowser.Infrastructure.Export;

public interface IFileTemplateEngine
{
	Task<MemoryStream> RenderAsync(FileStream fileStream, IReadOnlyDictionary<string, string> tokens);
}