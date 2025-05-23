using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.CompleteBloodCountAggregate.Specifications;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.ListUnderReview;

public class ListUnderReviewCompleteBloodCountsHandler(IReadRepository<CompleteBloodCount> _repository)
	: IQueryHandler<ListUnderReviewCompleteBloodCountsQuery,
		Result<IEnumerable<CompleteBloodCountDto>>>
{
	public async Task<Result<IEnumerable<CompleteBloodCountDto>>> Handle(ListUnderReviewCompleteBloodCountsQuery request,
		CancellationToken cancellationToken)
	{
		var spec = new UnderReviewCompleteBloodCountsSpec();
		var completeBloodCounts = await _repository.ListAsync(spec, cancellationToken);
		var dto = completeBloodCounts.Select(cbc => cbc.ConvertToCompleteBloodCountDto());

		return Result.Success(dto);
	}
}