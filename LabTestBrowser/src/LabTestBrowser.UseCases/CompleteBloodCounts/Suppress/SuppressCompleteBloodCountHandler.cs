using LabTestBrowser.Core.CompleteBloodCountAggregate;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Suppress;

public class SuppressCompleteBloodCountHandler(
	IRepository<CompleteBloodCount> _repository,
	ILogger<SuppressCompleteBloodCountHandler> _logger)
	: ICommandHandler<SuppressCompleteBloodCountCommand, Result>
{
	public async Task<Result> Handle(SuppressCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Suppressing complete blood count");

		if (!request.CompleteBloodCountId.HasValue)
			return Result.Invalid();

		var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId.Value, cancellationToken);
		if (cbc == null)
		{
			_logger.LogWarning("Missing required complete blood count id: {completeBloodCountId} in database",
				request.CompleteBloodCountId.Value);
			return Result.Error();
		}

		cbc.Suppress(request.SuppressionDate);
		_logger.LogInformation("Complete blood count id: {completeBloodCountId} suppressed at {suppressionDate}", cbc.Id,
			request.SuppressionDate);

		return Result.SuccessWithMessage("SuccessMessage.SuppressCompleteBloodCount");
	}
}