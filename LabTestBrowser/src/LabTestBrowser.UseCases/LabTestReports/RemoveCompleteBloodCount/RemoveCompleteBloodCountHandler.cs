using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReports.RemoveCompleteBloodCount;

public class RemoveCompleteBloodCountHandler(IRepository<LabTestReport> _repository)
	: ICommandHandler<RemoveCompleteBloodCountCommand, Result>
{
	public async Task<Result> Handle(RemoveCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		var spec = new LabTestReportBySpecimenSpec(request.Specimen, request.Date);
		var labTestReport = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (labTestReport == null)
			return Result.NotFound();

		labTestReport.RemoveCompleteBloodCount();
		await _repository.UpdateAsync(labTestReport, cancellationToken);

		return Result.Success();
	}
}