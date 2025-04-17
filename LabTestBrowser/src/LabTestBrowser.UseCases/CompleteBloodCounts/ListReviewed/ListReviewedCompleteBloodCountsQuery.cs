namespace LabTestBrowser.UseCases.CompleteBloodCounts.ListReviewed;

public record ListReviewedCompleteBloodCountsQuery(DateOnly ReviewDate) : IQuery<Result<IEnumerable<CompleteBloodCountDto>>>;