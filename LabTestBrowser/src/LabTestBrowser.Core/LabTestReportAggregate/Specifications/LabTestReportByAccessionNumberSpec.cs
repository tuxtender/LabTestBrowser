using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.Core.LabTestReportAggregate.Specifications;

public class LabTestReportByAccessionNumberSpec : Specification<LabTestReport>
{
	public LabTestReportByAccessionNumberSpec(AccessionNumber accessionNumber) =>
		Query
			.Where(report => report.AccessionNumber.SequenceNumber == accessionNumber.SequenceNumber &&
				report.AccessionNumber.Date == accessionNumber.Date);
}