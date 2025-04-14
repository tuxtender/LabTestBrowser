namespace LabTestBrowser.UseCases.LabTestReports;

public interface ILabTestReportQueryService
{
	Task<LabTestReportDto?> FindLastLabTestReportAsync(DateOnly date);
	Task<LabTestReportDto?> FindNextLabTestReportAsync(int specimenSequentialNumber, DateOnly date);
	Task<LabTestReportDto?> FindPreviousLabTestReportAsync(int specimenSequentialNumber, DateOnly date);
}