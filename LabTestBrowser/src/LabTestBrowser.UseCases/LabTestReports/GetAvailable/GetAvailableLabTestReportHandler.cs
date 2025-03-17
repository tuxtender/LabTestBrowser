using LabTestBrowser.UseCases.LabTestReports.GetLast;

namespace LabTestBrowser.UseCases.LabTestReports.GetAvailable;

public class GetAvailableLabTestReportHandler(ILabTestReportQueryService query)
	: IQueryHandler<GetAvailableLabTestReportQuery, Result<LabTestReportDTO>>
{
	public async Task<Result<LabTestReportDTO>> Handle(GetAvailableLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var labTestReport = await query.GetAvailableLabTestReportAsync(request.Date);
		return labTestReport;
	}
}