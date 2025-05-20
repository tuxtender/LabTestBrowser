using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.LabTestReports.Match;

public class MatchLabTestReportHandler(
	IReadRepository<LabTestReport> _repository,
	IErrorLocalizationService _errorLocalizer,
	ILogger<MatchLabTestReportHandler> _logger)
	: ICommandHandler<MatchLabTestReportCommand, Result>
{
	public async Task<Result> Handle(MatchLabTestReportCommand request, CancellationToken cancellationToken)
	{
		if (!request.Id.HasValue)
			return Result.Error(_errorLocalizer.LabTestReportNotSaved);

		var report = await _repository.GetByIdAsync(request.Id.Value, cancellationToken);
		if (report == null)
		{
			_logger.LogWarning("LabTestReport id: {labTestReportId} not found", request.Id);
			return Result.CriticalError(_errorLocalizer.ApplicationFault);
		}

		var accessionNumber = AccessionNumber.Create(request.OrderNumber, request.OrderDate);
		if (!accessionNumber.IsSuccess || report.AccessionNumber != accessionNumber)
			return Result.Error(_errorLocalizer.LabTestReportNotSaved);

		var specimenCollectionCenter = SpecimenCollectionCenter.Create(request.Facility, request.TradeName);
		if (!specimenCollectionCenter.IsSuccess || report.SpecimenCollectionCenter != specimenCollectionCenter)
			return Result.Error(_errorLocalizer.LabTestReportNotSaved);

		var age = Age.Create(request.AgeInYears, request.AgeInMonths, request.AgeInDays);
		if (!age.IsSuccess)
			return Result.Error(_errorLocalizer.LabTestReportNotSaved);

		var patient = Patient.Create(request.Animal, age, request.PetOwner, request.Nickname, request.Category, request.Breed);
		if (!patient.IsSuccess || report.Patient != patient)
			return Result.Error(_errorLocalizer.LabTestReportNotSaved);

		return Result.Success();
	}
}