namespace LabTestBrowser.UseCases.LabTestReports;

public interface IExportService
{
	Task<Result> ExportAsync(int labTestReportId, int labTestReportTemplateId);
}