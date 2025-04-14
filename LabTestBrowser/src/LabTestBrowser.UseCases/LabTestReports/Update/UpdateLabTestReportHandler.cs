using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UseCases.LabTestReports.Update;

public class UpdateLabTestReportHandler(IRepository<LabTestReport> _repository)
	: ICommandHandler<UpdateLabTestReportCommand, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(UpdateLabTestReportCommand request, CancellationToken cancellationToken)
	{
		var labTestReport = await _repository.GetByIdAsync(request.Id, cancellationToken);

		if (labTestReport == null)
			return Result.NotFound();

		var specimenCollectionCenter = new SpecimenCollectionCenter(request.Facility!, request.TradeName!);
		var age = Age.Create(request.AgeInYears, request.AgeInMonths, request.AgeInDays);
		var patient = Patient.Create(request.Animal!, age, request.PetOwner, request.Nickname, request.Category, request.Breed);

		if (!patient.IsSuccess)
			return Result.Error();

		labTestReport.SetSpecimenCollectionCenter(specimenCollectionCenter);
		labTestReport.SetPatient(patient);
		labTestReport.SetCompleteBloodCount(request.CompleteBloodCountId);
		await _repository.UpdateAsync(labTestReport, cancellationToken);

		return labTestReport.ConvertToLabTestReportDto();
	}
}