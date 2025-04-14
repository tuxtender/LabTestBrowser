namespace LabTestBrowser.Core.LabTestReportAggregate.Specifications;

public class LabTestReportsByDateSpec : Specification<LabTestReport>
{
	public LabTestReportsByDateSpec(DateOnly date) =>
		Query
			.Where(report => report.Specimen.Date == date);
}