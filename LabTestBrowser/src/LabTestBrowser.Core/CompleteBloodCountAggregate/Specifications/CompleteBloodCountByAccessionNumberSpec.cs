namespace LabTestBrowser.Core.CompleteBloodCountAggregate.Specifications;

public class CompleteBloodCountByAccessionNumberSpec : Specification<CompleteBloodCount>
{
	public CompleteBloodCountByAccessionNumberSpec(AccessionNumber accessionNumber) =>
		Query
			.Where(cbc => cbc.AccessionNumber!.SequenceNumber == accessionNumber.SequenceNumber &&
				cbc.AccessionNumber.Date == accessionNumber.Date);
}