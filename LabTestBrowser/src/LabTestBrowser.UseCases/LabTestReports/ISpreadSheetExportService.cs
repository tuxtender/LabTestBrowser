namespace LabTestBrowser.UseCases.LabTestReports;

public interface ISpreadSheetExportService
{
	Task Export(int labTestReportTemplateId, int labTestReportId);
}