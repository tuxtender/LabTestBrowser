using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReports.Save;

public class SaveLabTestReportHandler(IRepository<LabTestReport> _repository) : ICommandHandler<SaveLabTestReportCommand, Result<int>>
{
	public async Task<Result<int>> Handle(SaveLabTestReportCommand request, CancellationToken cancellationToken)
	{
		var specimen = new Specimen(request.Specimen, request.Date);
		var specimenCollectionCenter = new SpecimenCollectionCenter(request.Facility!, request.TradeName!);
		var age = Age.Create(request.AgeInYears, request.AgeInMonths, request.AgeInDays);
		var patient = Patient.Create(request.Animal!, age, request.PetOwner, request.Nickname, request.Category, request.Breed);

		var labTestReport = new LabTestReport(specimen, specimenCollectionCenter, patient);
		var spec = new LabTestReportByIdSpec(request.Id);
		var isReportCreated = await _repository.AnyAsync(spec, cancellationToken);

		if (isReportCreated)
			await _repository.UpdateAsync(labTestReport, cancellationToken);
		else
			labTestReport = await _repository.AddAsync(labTestReport, cancellationToken);

		return labTestReport.Id;
	}
}