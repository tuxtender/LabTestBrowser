using LabTestBrowser.UseCases.CompleteBloodCounts.Review;
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
			var updateResult = await UpdateReportAsync(request, cancellationToken);
			return updateResult;
		}

		var createResult = await CreateReportAsync(request, cancellationToken);
		return createResult;
	}

	private async Task<Result<LabTestReportDto>> UpdateReportAsync(SaveLabTestReportCommand request, CancellationToken cancellationToken)
	{
		var command = new UpdateLabTestReportCommand
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

		var result = await _mediator.Send(command, cancellationToken);
		return result;

		// if (!result.IsSuccess || !request.CompleteBloodCountId.HasValue)
		// 	return result;
		//
		// var reviewCommand = new ReviewCompleteBloodCountCommand(request.CompleteBloodCountId, request.OrderNumber, request.OrderDate);
		// var reviewResult = await _mediator.Send(reviewCommand, cancellationToken);
		//
		// return reviewResult.IsSuccess ? result : reviewResult;
	}

	private async Task<Result<LabTestReportDto>> CreateReportAsync(SaveLabTestReportCommand request, CancellationToken cancellationToken)
	{
		var command = new CreateLabTestReportCommand
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

		var result = await _mediator.Send(command, cancellationToken);
		return result;
		
		// if (!result.IsSuccess || !request.CompleteBloodCountId.HasValue)
		// 	return result;
		//
		// var reviewCommand = new ReviewCompleteBloodCountCommand(request.CompleteBloodCountId, request.OrderNumber, request.OrderDate);
		// var reviewResult = await _mediator.Send(reviewCommand, cancellationToken);
		//
		// return reviewResult.IsSuccess ? result : reviewResult;
	}
}