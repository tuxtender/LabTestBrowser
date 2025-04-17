namespace LabTestBrowser.UseCases.LabTestReports.GetEmpty;

public class GetEmptyLabTestReportHandler(ILabTestReportQueryService _query)
	: IQueryHandler<GetEmptyLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetEmptyLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var report = await _query.FindLastLabTestReportAsync(request.Date);

		if (report == null)
			return new LabTestReportDto
			{
				Date = request.Date,
				SequenceNumber = 1
			};

		var availableNumber = report.SequenceNumber + 1;

		return new LabTestReportDto
		{
			Date = request.Date,
			SequenceNumber = availableNumber
		};
	}
}