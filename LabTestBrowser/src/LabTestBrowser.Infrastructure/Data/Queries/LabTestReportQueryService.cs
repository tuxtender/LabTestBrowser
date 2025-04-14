using LabTestBrowser.UseCases.LabTestReports;

namespace LabTestBrowser.Infrastructure.Data.Queries;

public class LabTestReportQueryService(AppDbContext _dbContext) : ILabTestReportQueryService
{
	public async Task<LabTestReportDto?> FindLastLabTestReportAsync(DateOnly date)
	{
		var lastLabTestReport = await _dbContext.LabTestReports
			.Where(report => report.Specimen.Date == date)
			.OrderByDescending(report => report.Specimen.SequentialNumber)
			.FirstOrDefaultAsync();

		return lastLabTestReport?.ConvertToLabTestReportDto();
	}

	public async Task<LabTestReportDto?> FindNextLabTestReportAsync(int specimenSequentialNumber, DateOnly date)
	{
		//TODO: .AsNoTracking()

		var lastLabTestReport = await _dbContext.LabTestReports
			.Where(report => report.Specimen.Date == date)
			.Where(report => report.Specimen.SequentialNumber > specimenSequentialNumber)
			.OrderBy(report => report.Specimen.SequentialNumber)
			.FirstOrDefaultAsync();

		return lastLabTestReport?.ConvertToLabTestReportDto();
	}

	public async Task<LabTestReportDto?> FindPreviousLabTestReportAsync(int specimenSequentialNumber, DateOnly date)
	{
		var lastLabTestReport = await _dbContext.LabTestReports
			.Where(report => report.Specimen.Date == date)
			.Where(report => report.Specimen.SequentialNumber < specimenSequentialNumber)
			.OrderByDescending(report => report.Specimen.SequentialNumber)
			.FirstOrDefaultAsync();

		return lastLabTestReport?.ConvertToLabTestReportDto();
	}
}