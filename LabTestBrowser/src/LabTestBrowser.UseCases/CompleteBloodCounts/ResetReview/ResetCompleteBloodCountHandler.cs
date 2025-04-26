using LabTestBrowser.Core.CompleteBloodCountAggregate;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.ResetReview;

public class ResetCompleteBloodCountHandler(IRepository<CompleteBloodCount> _repository, ILogger<ResetCompleteBloodCountHandler> _logger)
	: ICommandHandler<ResetCompleteBloodCountCommand, Result>
{
	public async Task<Result> Handle(ResetCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		if (!request.CompleteBloodCountId.HasValue)
			return Result.Invalid(new ValidationError("ValidationError.TestNotSelected", "No test selected"));

		var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId.Value, cancellationToken);
		if (cbc == null)
		{
			_logger.LogWarning("Missing required complete blood count id: {completeBloodCountId} in database",
				request.CompleteBloodCountId.Value);
			return Result.CriticalError("ErrorMessage.ApplicationFault");
		}

		cbc.Review();
		await _repository.UpdateAsync(cbc, cancellationToken);
		_logger.LogInformation("Complete blood count id: {completeBloodCountId} set under review", cbc.Id);

		return Result.SuccessWithMessage("SuccessMessage.ResetCompleteBloodCount");
	}
}