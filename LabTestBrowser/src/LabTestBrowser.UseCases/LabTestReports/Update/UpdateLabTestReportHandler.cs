using LabTestBrowser.Core.LabTestReportAggregate;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.LabTestReports.Update;

public class UpdateLabTestReportHandler(
	IRepository<LabTestReport> _repository,
	IValidationLocalizationService _validationLocalizer,
	IErrorLocalizationService _errorLocalizer,
	ILogger<UpdateLabTestReportHandler> _logger)
	: ICommandHandler<UpdateLabTestReportCommand, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(UpdateLabTestReportCommand request, CancellationToken cancellationToken)
	{
		var labTestReport = await _repository.GetByIdAsync(request.Id, cancellationToken);
		if (labTestReport == null)
		{
			_logger.LogWarning("LabTestReport id: {labTestReportId} not found", request.Id);
			return Result.CriticalError(_errorLocalizer.ApplicationFault);
		}

		var specimenCollectionCenter = SpecimenCollectionCenter.Create(request.Facility, request.TradeName!);
		if (!specimenCollectionCenter.IsSuccess)
			return Result.Invalid(_validationLocalizer.Localize(specimenCollectionCenter.ValidationErrors));

		var age = Age.Create(request.AgeInYears, request.AgeInMonths, request.AgeInDays);
		if (!age.IsSuccess)
			return Result.Invalid(_validationLocalizer.Localize(age.ValidationErrors));

		var patient = Patient.Create(request.Animal, age, request.PetOwner, request.Nickname, request.Category, request.Breed);
		if (!patient.IsSuccess)
			return Result.Invalid(_validationLocalizer.Localize(patient.ValidationErrors));

		labTestReport.UpdateSpecimenCollectionCenter(specimenCollectionCenter);
		labTestReport.UpdatePatient(patient);
		await _repository.UpdateAsync(labTestReport, cancellationToken);
		_logger.LogInformation("Updated LabTestReport id: {labTestReportId}", request.Id);

		return labTestReport.ConvertToLabTestReportDto();
	}
}