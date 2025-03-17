namespace LabTestBrowser.Core.LabTestReportAggregate.Specifications;

public class LabTestReportByIdSpec : Specification<LabTestReport>
{
	public LabTestReportByIdSpec(int labTestReportId) =>
		Query
			.Where(report => report.Id == labTestReportId);
}