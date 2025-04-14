using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UseCases.LabTestReports.GetPrevious;

public class GetPreviousLabTestReportHandler(ILabTestReportQueryService _query, IReadRepository<LabTestReport> _repository)
	: IQueryHandler<GetPreviousLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetPreviousLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var labTestReport = await _repository.GetByIdAsync(request.LabTestReportId, cancellationToken);

		if (labTestReport == null)
			return Result.Error("LabTestReport not found");

		var previousLabTestReport =
			await _query.FindPreviousLabTestReportAsync(labTestReport.Specimen.SequentialNumber, labTestReport.Specimen.Date);

		return previousLabTestReport ?? labTestReport.ConvertToLabTestReportDTO();
	}
}