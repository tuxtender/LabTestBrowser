namespace LabTestBrowser.UseCases.Export;

public interface IExportFileNamingService
{
	Task<string> GetExportPathAsync(IReadOnlyDictionary<string, string> tokens, string fileExtension);
}