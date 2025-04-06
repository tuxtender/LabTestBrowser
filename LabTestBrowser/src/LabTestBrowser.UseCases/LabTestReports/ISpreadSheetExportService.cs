namespace LabTestBrowser.UseCases.LabTestReports;

public interface ISpreadSheetExportService
{
	Task Export(LabTestReportTemplate reportTemplate, int labTestReportId);
}