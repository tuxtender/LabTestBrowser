using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UseCases.LabTestReports.Update;
using MediatR;

namespace LabTestBrowser.UseCases.LabTestReports.Save;

public class SaveLabTestReportHandler(IRepository<LabTestReport> _repository, IMediator _mediator)
	: ICommandHandler<SaveLabTestReportCommand, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(SaveLabTestReportCommand request, CancellationToken cancellationToken)
	{
		if (request.Id.HasValue)
			return await UpdateLabTestReportAsync(request);

		var specimen = new Specimen(request.Specimen, request.Date);
		var specimenCollectionCenter = new SpecimenCollectionCenter(request.Facility!, request.TradeName!);
		var age = Age.Create(request.AgeInYears, request.AgeInMonths, request.AgeInDays);
		var patient = Patient.Create(request.Animal!, age, request.PetOwner, request.Nickname, request.Category, request.Breed);

		if (!patient.IsSuccess)
			return Result.Error();

		var labTestReport = new LabTestReport(specimen, specimenCollectionCenter, patient);
		labTestReport.SetCompleteBloodCount(request.CompleteBloodCountId);
		labTestReport = await _repository.AddAsync(labTestReport, cancellationToken);

		return labTestReport.ConvertToLabTestReportDto();
	}

	private async Task<Result<LabTestReportDto>> UpdateLabTestReportAsync(SaveLabTestReportCommand request)
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
			AgeInDays = request.AgeInDays,
			CompleteBloodCountId = request.CompleteBloodCountId,
		};

		var report = await _mediator.Send(command);

		return report;
	}
}