using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Get;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetCompleteBloodCountHandler(IReadRepository<CompleteBloodCount> _repository)
	: IQueryHandler<GetCompleteBloodCountQuery, Result<CompleteBloodCountDto>>
{
	public async Task<Result<CompleteBloodCountDto>> Handle(GetCompleteBloodCountQuery request, CancellationToken cancellationToken)
	{
		var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId, cancellationToken);

		if (cbc == null)
			return Result.NotFound();

		var dto = cbc.ConvertToCompleteBloodCountDto();

		return dto;
	}
}