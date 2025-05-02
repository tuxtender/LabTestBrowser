namespace LabTestBrowser.UseCases.Export;

public interface IExportService
{
	Task<Result> ExportAsync(int labTestReportId, int labTestReportTemplateId);
}