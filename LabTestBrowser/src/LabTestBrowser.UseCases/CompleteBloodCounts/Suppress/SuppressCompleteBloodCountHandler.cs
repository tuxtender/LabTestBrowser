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
		if (!request.CompleteBloodCountId.HasValue)
			return Result.Invalid(new ValidationError("TestNotSelected"));

		var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId.Value, cancellationToken);
		if (cbc == null)
		{
			_logger.LogWarning("Missing required complete blood count id: {completeBloodCountId} in database",
				request.CompleteBloodCountId.Value);
			return Result.CriticalError("ApplicationFault");
		}

		cbc.Suppress(request.SuppressionDate);
		await _repository.UpdateAsync(cbc, cancellationToken);
		_logger.LogInformation("Complete blood count id: {completeBloodCountId} suppressed at {suppressionDate}", cbc.Id,
			request.SuppressionDate);

		return Result.Success();
	}
}