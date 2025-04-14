using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UseCases.LabTestReports.Get;

public class GetLastLabTestReportHandler(IReadRepository<LabTestReport> _repository)
	: IQueryHandler<GetLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var labTestReport = await _repository.GetByIdAsync(request.LabTestReportId, cancellationToken);

		if (labTestReport == null)
			return Result.NotFound();

		var dto = labTestReport.ConvertToLabTestReportDto();

		return dto;
	}
}