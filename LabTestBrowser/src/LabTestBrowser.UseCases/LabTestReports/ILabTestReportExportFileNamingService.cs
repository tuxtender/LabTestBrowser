namespace LabTestBrowser.UseCases.LabTestReports;

public interface ILabTestReportExportFileNamingService
{
	Task<string> GetExportFilenameAsync(int labTestReportId, int labTestReportTemplateId);
}