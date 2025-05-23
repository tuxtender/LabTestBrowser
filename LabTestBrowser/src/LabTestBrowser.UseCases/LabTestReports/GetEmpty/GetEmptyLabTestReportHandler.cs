namespace LabTestBrowser.UseCases.LabTestReports.GetEmpty;

public class GetEmptyLabTestReportHandler(ILabTestReportQueryService _query)
	: IQueryHandler<GetEmptyLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetEmptyLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var report = await _query.FindLastLabTestReportAsync(request.OrderDate);
		if (report == null)
			return new LabTestReportDto
			{
				OrderDate = request.OrderDate,
				OrderNumber = 1
			};

		var availableNumber = report.OrderNumber + 1;

		return new LabTestReportDto
		{
			OrderDate = request.OrderDate,
			OrderNumber = availableNumber
		};
	}
}