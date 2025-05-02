namespace LabTestBrowser.Infrastructure.Export;

public interface IFileTemplateEngine
{
	Task<MemoryStream> RenderAsync(FileStream fileStream, Dictionary<string, string> tokens);
}