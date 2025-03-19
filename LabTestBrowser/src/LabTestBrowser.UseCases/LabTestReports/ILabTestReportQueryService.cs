namespace LabTestBrowser.UseCases.LabTestReports;

public interface ILabTestReportQueryService
{
	Task<LabTestReportDTO?> FindLastLabTestReportAsync(DateOnly date);
	Task<LabTestReportDTO?> FindNextLabTestReportAsync(int specimenSequentialNumber, DateOnly date);
	Task<LabTestReportDTO?> FindPreviousLabTestReportAsync(int specimenSequentialNumber, DateOnly date);
}