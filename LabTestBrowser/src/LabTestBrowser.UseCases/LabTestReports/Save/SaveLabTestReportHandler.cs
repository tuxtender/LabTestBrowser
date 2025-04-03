using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UseCases.LabTestReports.Save;

public class SaveLabTestReportHandler(IRepository<LabTestReport> _repository) : ICommandHandler<SaveLabTestReportCommand, Result<int>>
{
	public async Task<Result<int>> Handle(SaveLabTestReportCommand request, CancellationToken cancellationToken)
	{
		var labTestReport = await _repository.GetByIdAsync(request.Id, cancellationToken);
		
		var specimen = new Specimen(request.Specimen, request.Date);
		var specimenCollectionCenter = new SpecimenCollectionCenter(request.Facility!, request.TradeName!);
		var age = Age.Create(request.AgeInYears, request.AgeInMonths, request.AgeInDays);
		var patient = Patient.Create(request.Animal!, age, request.PetOwner, request.Nickname, request.Category, request.Breed);
		
		if(!patient.IsSuccess)
			return Result.Error(new ErrorList(patient.Errors));

		if (labTestReport == null)
		{
			labTestReport = new LabTestReport(specimen, specimenCollectionCenter, patient);
			labTestReport.SetCompleteBloodCount(request.CompleteBloodCountId);
			labTestReport = await _repository.AddAsync(labTestReport, cancellationToken);
			return labTestReport.Id;
		}

		if(labTestReport.Specimen != specimen)
			return Result.Error("Report overwriting is not allowed");
		
		labTestReport.SetSpecimenCollectionCenter(specimenCollectionCenter);
		labTestReport.SetPatient(patient);
		labTestReport.SetCompleteBloodCount(request.CompleteBloodCountId);
		await _repository.UpdateAsync(labTestReport, cancellationToken);

		return labTestReport.Id;
	}
}