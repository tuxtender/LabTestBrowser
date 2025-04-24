using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReports.Create;

public class CreateLabTestReportHandler(IRepository<LabTestReport> _repository)
	: ICommandHandler<CreateLabTestReportCommand, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(CreateLabTestReportCommand request, CancellationToken cancellationToken)
	{
		var accessionNumber = AccessionNumber.Create(request.SequenceNumber, request.Date);

		if (!accessionNumber.IsSuccess)
			return Result.Error();

		var spec = new LabTestReportByAccessionNumberSpec(accessionNumber);
		var labTestReport = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (labTestReport != null)
			return Result.Error();

		var specimenCollectionCenter = SpecimenCollectionCenter.Create(request.Facility, request.TradeName!);

		if (!specimenCollectionCenter.IsSuccess)
			return Result.Error();

		var age = Age.Create(request.AgeInYears, request.AgeInMonths, request.AgeInDays);

		if (!age.IsSuccess)
			return Result.Error();

		var patient = Patient.Create(request.Animal, age, request.PetOwner, request.Nickname, request.Category, request.Breed);

		if (!patient.IsSuccess)
			return Result.Error();

		labTestReport = new LabTestReport(accessionNumber, specimenCollectionCenter, patient);

		await _repository.AddAsync(labTestReport, cancellationToken);

		return labTestReport.ConvertToLabTestReportDto();
	}
}