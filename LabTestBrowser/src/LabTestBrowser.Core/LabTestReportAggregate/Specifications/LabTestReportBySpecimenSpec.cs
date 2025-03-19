namespace LabTestBrowser.Core.LabTestReportAggregate.Specifications;

public class LabTestReportBySpecimenSpec : Specification<LabTestReport>
{
	public LabTestReportBySpecimenSpec(int specimenSequentialNumber, DateOnly collectionDate) =>
		Query
			.Where(report => report.Specimen.SequentialNumber == specimenSequentialNumber && report.Specimen.Date == collectionDate);
}