using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.LabTestReports.Create;

public class CreateLabTestReportHandler(
	IRepository<LabTestReport> _repository,
	IErrorLocalizationService _errorLocalizer,
	IValidationLocalizationService _validationLocalizer,
	ILogger<CreateLabTestReportHandler> _logger)
	: ICommandHandler<CreateLabTestReportCommand, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(CreateLabTestReportCommand request, CancellationToken cancellationToken)
	{
		var accessionNumber = AccessionNumber.Create(request.OrderNumber, request.OrderDate);
		if (!accessionNumber.IsSuccess)
		{
			_logger.LogWarning("Invalid values for AccessionNumber: {sequenceNumber} {date}", request.OrderNumber, request.OrderDate);
			return Result.Invalid(_validationLocalizer.Localize(accessionNumber.ValidationErrors));
		}

		var spec = new LabTestReportByAccessionNumberSpec(accessionNumber);
		var labTestReport = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
		if (labTestReport != null)
		{
			_logger.LogWarning("LabTestReport id: {labTestReportId} is already created", labTestReport.Id);
			return Result.Conflict(_errorLocalizer.GetLabTestReportIdConflict());
		}

		var specimenCollectionCenter = SpecimenCollectionCenter.Create(request.Facility, request.TradeName!);
		if (!specimenCollectionCenter.IsSuccess)
			return Result.Invalid(_validationLocalizer.Localize(specimenCollectionCenter.ValidationErrors));

		var age = Age.Create(request.AgeInYears, request.AgeInMonths, request.AgeInDays);
		if (!age.IsSuccess)
			return Result.Invalid(age.ValidationErrors);

		var patient = Patient.Create(request.Animal, age, request.PetOwner, request.Nickname, request.Category, request.Breed);
		if (!patient.IsSuccess)
			return Result.Invalid(_validationLocalizer.Localize(patient.ValidationErrors));

		labTestReport = new LabTestReport(accessionNumber, specimenCollectionCenter, patient);
		await _repository.AddAsync(labTestReport, cancellationToken);
		_logger.LogInformation("Created LabTestReport for accession number: {sequenceNumber} {date}", request.OrderNumber,
			request.OrderDate);

		return labTestReport.ConvertToLabTestReportDto();
	}
}