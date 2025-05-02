namespace LabTestBrowser.UseCases.Export;

public interface IExportFileNamingService
{
	Task<string> GetExportPathAsync(Dictionary<string, string> tokens, string fileExtension);
}