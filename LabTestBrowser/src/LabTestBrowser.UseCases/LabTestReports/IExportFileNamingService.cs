namespace LabTestBrowser.UseCases.LabTestReports;

public interface IExportFileNamingService
{
	Task<string> GetExportPathAsync(Dictionary<string, string> tokens, string fileExtension);
}