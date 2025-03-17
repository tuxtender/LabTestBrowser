namespace LabTestBrowser.UseCases.LabTestReports.GetLast;

public class GetLastLabTestReportHandler(ILabTestReportQueryService query)
	: IQueryHandler<GetLastLabTestReportQuery, Result<LabTestReportDTO>>
{
	public async Task<Result<LabTestReportDTO>> Handle(GetLastLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var labTestReport = await query.FindLastLabTestReportAsync(request.Date);
		
		if(labTestReport == null)
			return Result.NotFound();
		
		return labTestReport;
	}
}