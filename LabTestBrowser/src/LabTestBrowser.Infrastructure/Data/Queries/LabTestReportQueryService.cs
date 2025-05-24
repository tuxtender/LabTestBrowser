using LabTestBrowser.UseCases.LabTestReports;

namespace LabTestBrowser.Infrastructure.Data.Queries;

public class LabTestReportQueryService(AppDbContext _dbContext) : ILabTestReportQueryService
{
	public async Task<LabTestReportDto?> FindLastLabTestReportAsync(DateOnly date)
	{
		var lastLabTestReport = await _dbContext.LabTestReports
			.Where(report => report.AccessionNumber.Date == date)
			.OrderByDescending(report => report.AccessionNumber.SequenceNumber)
			.FirstOrDefaultAsync();

		return lastLabTestReport?.ConvertToLabTestReportDto();
	}

	public async Task<LabTestReportDto?> FindNextLabTestReportAsync(int sequenceNumber, DateOnly date)
	{
		var lastLabTestReport = await _dbContext.LabTestReports
			.Where(report => report.AccessionNumber.Date == date)
			.Where(report => report.AccessionNumber.SequenceNumber > sequenceNumber)
			.OrderBy(report => report.AccessionNumber.SequenceNumber)
			.FirstOrDefaultAsync();

		return lastLabTestReport?.ConvertToLabTestReportDto();
	}

	public async Task<LabTestReportDto?> FindPreviousLabTestReportAsync(int sequenceNumber, DateOnly date)
	{
		var lastLabTestReport = await _dbContext.LabTestReports
			.Where(report => report.AccessionNumber.Date == date)
			.Where(report => report.AccessionNumber.SequenceNumber < sequenceNumber)
			.OrderByDescending(report => report.AccessionNumber.SequenceNumber)
			.FirstOrDefaultAsync();

		return lastLabTestReport?.ConvertToLabTestReportDto();
	}
}