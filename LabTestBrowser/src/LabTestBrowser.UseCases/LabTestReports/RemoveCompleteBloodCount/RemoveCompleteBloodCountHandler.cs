using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UseCases.LabTestReports.RemoveCompleteBloodCount;

public class RemoveCompleteBloodCountHandler(IRepository<LabTestReport> _repository)
	: ICommandHandler<RemoveCompleteBloodCountCommand, Result>
{
	public async Task<Result> Handle(RemoveCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		var report = await _repository.GetByIdAsync(request.LabTestReportId, cancellationToken);

		if (report == null)
			return Result.NotFound();

		report.RemoveCompleteBloodCount();
		await _repository.UpdateAsync(report, cancellationToken);

		return Result.Success();
	}
}