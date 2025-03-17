using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.LabTestReports.GetLast;

namespace LabTestBrowser.Infrastructure.Data.Queries;

public class LabTestReportQueryService(AppDbContext dbContext) : ILabTestReportQueryService
{
	public async Task<LabTestReportDTO?> FindLastLabTestReportAsync(DateOnly date)
	{
		var lastLabTestReport = await dbContext.LabTestReports
			.Where(report => report.Specimen.Date == date)
			.OrderByDescending(report => report.Specimen.Date)
			.FirstOrDefaultAsync();

		if (lastLabTestReport == null)
			return null;

		return new LabTestReportDTO
		{
			Animal = lastLabTestReport.Patient.Animal,
			SequentialNumber = lastLabTestReport.Specimen.SequentialNumber,
			Date = lastLabTestReport.Specimen.Date,
			Facility = lastLabTestReport.SpecimenCollectionCenter.Facility,
			TradeName = lastLabTestReport.SpecimenCollectionCenter.TradeName,
			HealthcareProxy = lastLabTestReport.Patient.HealthcareProxy,
			Name = lastLabTestReport.Patient.Name,
			Category = lastLabTestReport.Patient.Category,
			Breed = lastLabTestReport.Patient.Breed,
			AgeInYears = lastLabTestReport.Patient.Age.Years,
			AgeInMonths = lastLabTestReport.Patient.Age.Months,
			AgeInDays = lastLabTestReport.Patient.Age.Days,
			CompleteBloodCountId = lastLabTestReport.CompleteBloodCountId,
		};
	}

	public async Task<LabTestReportDTO> GetAvailableLabTestReportAsync(DateOnly date)
	{
		var labTestReport = await FindLastLabTestReportAsync(date);

		if (labTestReport == null)
			return new LabTestReportDTO
			{
				Date = date,
				SequentialNumber = 0
			};

		var availableNumber = labTestReport.SequentialNumber + 1;

		return new LabTestReportDTO
		{
			Date = date,
			SequentialNumber = availableNumber
		};
	}
}