namespace LabTestBrowser.Core.CompleteBloodCountAggregate.Specifications;

public class UnderReviewCompleteBloodCountsSpec : Specification<CompleteBloodCount>
{
	public UnderReviewCompleteBloodCountsSpec() =>
		Query
			.Where(cbc => cbc.ReviewResult == ReviewResult.UnderReview);
}