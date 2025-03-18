namespace LabTestBrowser.UseCases.LabTestReports;

public interface ILabTestReportQueryService
{
	Task<LabTestReportDTO?> FindLastLabTestReportAsync(DateOnly date);
}