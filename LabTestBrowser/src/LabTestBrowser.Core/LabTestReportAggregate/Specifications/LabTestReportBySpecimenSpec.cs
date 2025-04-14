namespace LabTestBrowser.Core.LabTestReportAggregate.Specifications;

public class LabTestReportBySpecimenSpec : Specification<LabTestReport>
{
	public LabTestReportBySpecimenSpec(int specimen, DateOnly date) =>
		Query
			.Where(report => report.Specimen.SequentialNumber == specimen && report.Specimen.Date == date);
}