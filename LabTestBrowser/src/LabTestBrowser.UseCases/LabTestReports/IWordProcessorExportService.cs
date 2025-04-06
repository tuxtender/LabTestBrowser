namespace LabTestBrowser.UseCases.LabTestReports;

public interface IWordProcessorExportService
{
	Task Export(LabTestReportTemplate reportTemplate, int labTestReportId);
}