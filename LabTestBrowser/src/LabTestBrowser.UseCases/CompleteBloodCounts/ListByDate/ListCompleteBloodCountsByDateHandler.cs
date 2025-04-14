using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.ListByDate;

public class ListCompleteBloodCountsByDateHandler(
	IReadRepository<LabTestReport> _reportRepository,
	IReadRepository<CompleteBloodCount> _cbcRepository)
	: IQueryHandler<ListCompleteBloodCountsByDateQuery, Result<IEnumerable<CompleteBloodCountDto>>>
{
	public async Task<Result<IEnumerable<CompleteBloodCountDto>>> Handle(ListCompleteBloodCountsByDateQuery request,
		CancellationToken cancellationToken)
	{
		var spec = new LabTestReportsByDateSpec(request.Date);
		var reports = await _reportRepository.ListAsync(spec, cancellationToken);

		var completeBloodCountGettingTasks = reports
			.Where(report => report.CompleteBloodCountId.HasValue)
			.Select(report => report.CompleteBloodCountId)
			.Cast<int>()
			.Select(id => _cbcRepository.GetByIdAsync(id, cancellationToken));

		var completeBloodCounts = await Task.WhenAll(completeBloodCountGettingTasks);
		var dto = completeBloodCounts.Select(cbc => cbc!.ConvertToCompleteBloodCountDto());

		return Result.Success(dto);
	}
}