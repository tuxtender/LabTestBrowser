using LabTestBrowser.UseCases.LabTestReports.Create;
using LabTestBrowser.UseCases.LabTestReports.Update;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.LabTestReports.Save;

public class SaveLabTestReportHandler(
	IMediator _mediator,
	ILocalizationService _localizationService,
	ILogger<SaveLabTestReportHandler> _logger)
	: ICommandHandler<SaveLabTestReportCommand, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(SaveLabTestReportCommand request, CancellationToken cancellationToken)
	{
		if (request.Id.HasValue)
		{
			var updateCommand = CreateUpdateLabTestReportCommand(request);
			var updateResult = await _mediator.Send(updateCommand, cancellationToken);

			return updateResult.IsSuccess ? GetSuccessMessageResult(updateResult) : updateResult;
		}

		var createCommand = new CreateLabTestReportCommand
		{
			SequenceNumber = request.SequenceNumber,
			Date = request.Date,
			Facility = request.Facility,
			TradeName = request.TradeName,
			PetOwner = request.PetOwner,
			Nickname = request.Nickname,
			Animal = request.Animal,
			Category = request.Category,
			Breed = request.Breed,
			AgeInYears = request.AgeInYears,
			AgeInMonths = request.AgeInMonths,
			AgeInDays = request.AgeInDays
		};

		var createResult = await _mediator.Send(createCommand, cancellationToken);
		return createResult.IsSuccess ? GetSuccessMessageResult(createResult) : createResult;
	}

	private Result<LabTestReportDto> GetSuccessMessageResult(Result<LabTestReportDto> result)
	{
		_logger.LogInformation("Saved successfully");
		var message = _localizationService.GetString("SaveLabTestReport_Saved");
		return Result.Success(result.Value, message);
	}

	private static UpdateLabTestReportCommand CreateUpdateLabTestReportCommand(SaveLabTestReportCommand request)
	{
		return new UpdateLabTestReportCommand
		{
			Id = request.Id!.Value,
			Facility = request.Facility,
			TradeName = request.TradeName,
			PetOwner = request.PetOwner,
			Nickname = request.Nickname,
			Animal = request.Animal,
			Category = request.Category,
			Breed = request.Breed,
			AgeInYears = request.AgeInYears,
			AgeInMonths = request.AgeInMonths,
			AgeInDays = request.AgeInDays
		};
	}
}