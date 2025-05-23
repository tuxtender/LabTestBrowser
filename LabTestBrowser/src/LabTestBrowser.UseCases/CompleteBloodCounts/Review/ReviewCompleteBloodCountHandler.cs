using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.CompleteBloodCountAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Review;

public class ReviewCompleteBloodCountHandler(
	IRepository<CompleteBloodCount> _repository,
	IErrorLocalizationService _errorLocalizer,
	IValidationLocalizationService _validationLocalizer,
	ILogger<ReviewCompleteBloodCountHandler> _logger)
	: ICommandHandler<ReviewCompleteBloodCountCommand, Result>
{
	public async Task<Result> Handle(ReviewCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		if (!request.CompleteBloodCountId.HasValue)
			return Result.Invalid(new ValidationError(_errorLocalizer.TestNotSelected));

		var accessionNumber = AccessionNumber.Create(request.LabOrderNumber, request.LabOrderDate);
		if (!accessionNumber.IsSuccess)
		{
			_logger.LogWarning("Invalid values for AccessionNumber: {sequenceNumber} {date}", request.LabOrderNumber, request.LabOrderDate);
			return Result.Invalid(_validationLocalizer.Localize(accessionNumber.ValidationErrors));
		}

		var spec = new CompleteBloodCountByAccessionNumberSpec(accessionNumber);
		var assignedCbc = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
		if (assignedCbc != null)
		{
			assignedCbc.Suppress(request.LabOrderDate);
			await _repository.UpdateAsync(assignedCbc, cancellationToken);
			_logger.LogInformation("Complete blood count id: {completeBloodCountId} suppressed", assignedCbc.Id);
		}

		var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId.Value, cancellationToken);
		if (cbc == null)
		{
			_logger.LogWarning("Missing required complete blood count id: {completeBloodCountId} in database",
				request.CompleteBloodCountId.Value);
			return Result.CriticalError(_errorLocalizer.ApplicationFault);
		}

		cbc.Review(accessionNumber);
		await _repository.UpdateAsync(cbc, cancellationToken);
		_logger.LogInformation("Complete blood count id: {completeBloodCountId} reviewed", cbc.Id);

		return Result.Success();
	}
}