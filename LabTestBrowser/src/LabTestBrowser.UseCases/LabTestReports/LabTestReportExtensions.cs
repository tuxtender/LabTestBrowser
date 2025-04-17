using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UseCases.LabTestReports;

public static class LabTestReportExtensions
{
	public static LabTestReportDto ConvertToLabTestReportDto(this LabTestReport report)
	{
		return new LabTestReportDto
		{
			Id = report.Id,
			Animal = report.Patient.Animal,
			SequenceNumber = report.AccessionNumber.SequenceNumber,
			Date = report.AccessionNumber.Date,
			Facility = report.SpecimenCollectionCenter.Facility,
			TradeName = report.SpecimenCollectionCenter.TradeName,
			HealthcareProxy = report.Patient.HealthcareProxy,
			Name = report.Patient.Name,
			Category = report.Patient.Category,
			Breed = report.Patient.Breed,
			AgeInYears = report.Patient.Age.Years,
			AgeInMonths = report.Patient.Age.Months,
			AgeInDays = report.Patient.Age.Days
		};
	}
}