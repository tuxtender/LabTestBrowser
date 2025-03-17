namespace LabTestBrowser.UseCases.LabTestReports.GetLast;

public interface ILabTestReportQueryService
{
	Task<LabTestReportDTO?> FindLastLabTestReportAsync(DateOnly date);
	Task<LabTestReportDTO> GetAvailableLabTestReportAsync(DateOnly date);
}