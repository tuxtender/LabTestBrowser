using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.CompleteBloodCountAggregate.Specifications;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.ListReviewed;

public class ListReviewedCompleteBloodCountsHandler(IReadRepository<CompleteBloodCount> _repository)
	: IQueryHandler<ListReviewedCompleteBloodCountsQuery,
		Result<IEnumerable<CompleteBloodCountDto>>>
{
	public async Task<Result<IEnumerable<CompleteBloodCountDto>>> Handle(ListReviewedCompleteBloodCountsQuery request,
		CancellationToken cancellationToken)
	{
		var spec = new CompleteBloodCountsByReviewDateSpec(request.ReviewDate);
		var completeBloodCounts = await _repository.ListAsync(spec, cancellationToken);
		var dto = completeBloodCounts.Select(cbc => cbc.ConvertToCompleteBloodCountDto());

		return Result.Success(dto);
	}
}