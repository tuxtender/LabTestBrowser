using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdated;

public class GetUpdatedCompleteBloodCountHandler(
	IReadRepository<CompleteBloodCount> _repository,
	ICompleteBloodCountUpdateChannel _updateChannel,
	ILogger<GetUpdatedCompleteBloodCountHandler> _logger)
	: IQueryHandler<GetUpdatedCompleteBloodCountQuery, Result<CompleteBloodCountDto>>
{
	public async Task<Result<CompleteBloodCountDto>> Handle(GetUpdatedCompleteBloodCountQuery request, CancellationToken cancellationToken)
	{
		var completeBloodCountId = await _updateChannel.ReadAsync();
		var cbc = await _repository.GetByIdAsync(completeBloodCountId, cancellationToken);

		if (cbc != null)
			return cbc.ConvertToCompleteBloodCountDto();

		_logger.LogWarning("Inconsistent data. CBC: {completeBloodCountId} hasn't been saved yet", completeBloodCountId);
		return Result.Error();
	}
}