using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.Interfaces;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.GetCreated;

public class GetCreatedCompleteBloodCountHandler(
	IReadRepository<CompleteBloodCount> _repository,
	ICompleteBloodCountUpdateChannel _updateChannel)
	: IQueryHandler<GetCreatedCompleteBloodCountQuery, Result<CompleteBloodCountDto>>
{
	public async Task<Result<CompleteBloodCountDto>> Handle(GetCreatedCompleteBloodCountQuery request, CancellationToken cancellationToken)
	{
		var completeBloodCountId = await _updateChannel.ReadAsync();
		var cbc = await _repository.GetByIdAsync(completeBloodCountId, cancellationToken);

		return cbc!.ConvertToCompleteBloodCountDto();
	}
}