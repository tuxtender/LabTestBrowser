using LabTestBrowser.UseCases.LabTestReports.Create;
using LabTestBrowser.UseCases.LabTestReports.Update;
using MediatR;

namespace LabTestBrowser.UseCases.LabTestReports.Save;

public class SaveLabTestReportHandler(IMediator _mediator)
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
			OrderNumber = request.OrderNumber,
			OrderDate = request.OrderDate,
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

	private static Result<LabTestReportDto> GetSuccessMessageResult(Result<LabTestReportDto> result) =>
		Result.Success(result.Value, "SuccessMessage.SaveLabTestReport");
}