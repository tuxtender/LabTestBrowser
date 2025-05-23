namespace LabTestBrowser.UseCases.LabTestReports.GetLast;

public class GetLastLabTestReportHandler(ILabTestReportQueryService _query)
	: IQueryHandler<GetLastLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetLastLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var labTestReport = await _query.FindLastLabTestReportAsync(request.OrderDate);
		if (labTestReport == null)
			return new LabTestReportDto
			{
				OrderDate = request.OrderDate,
				OrderNumber = 1
			};

		return labTestReport;
	}
}