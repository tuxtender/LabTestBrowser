namespace LabTestBrowser.UseCases.LabTestReports;

public interface ILabTestReportQueryService
{
	Task<LabTestReportDto?> FindLastLabTestReportAsync(DateOnly date);
	Task<LabTestReportDto?> FindNextLabTestReportAsync(int sequenceNumber, DateOnly date);
	Task<LabTestReportDto?> FindPreviousLabTestReportAsync(int sequenceNumber, DateOnly date);
}