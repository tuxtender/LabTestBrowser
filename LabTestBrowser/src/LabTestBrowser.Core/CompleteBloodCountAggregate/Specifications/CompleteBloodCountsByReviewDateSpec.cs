namespace LabTestBrowser.Core.CompleteBloodCountAggregate.Specifications;

public class CompleteBloodCountsByReviewDateSpec : Specification<CompleteBloodCount>
{
	public CompleteBloodCountsByReviewDateSpec(DateOnly reviewDate) =>
		Query
			.Where(cbc => cbc.ReviewDate == reviewDate);
}