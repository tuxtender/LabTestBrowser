using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UseCases.LabTestReports.GetNext;

public class GetNextLabTestReportHandler(ILabTestReportQueryService _query, IReadRepository<LabTestReport> _repository)
	: IQueryHandler<GetNextLabTestReportQuery, Result<LabTestReportDTO>>
{
	public async Task<Result<LabTestReportDTO>> Handle(GetNextLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var labTestReport = await _repository.GetByIdAsync(request.LabTestReportId, cancellationToken);

		if (labTestReport == null)
			return Result.Error("LabTestReport not found");

		var nextLabTestReport =
			await _query.FindNextLabTestReportAsync(labTestReport.Specimen.SequentialNumber, labTestReport.Specimen.Date);

		return nextLabTestReport ?? labTestReport.ConvertToLabTestReportDTO();
	}
}