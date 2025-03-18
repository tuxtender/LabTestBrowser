namespace LabTestBrowser.UseCases.LabTestReports.GetEmpty;

public class GetEmptyLabTestReportHandler(ILabTestReportQueryService query)
	: IQueryHandler<GetEmptyLabTestReportQuery, Result<LabTestReportDTO>>
{
	public async Task<Result<LabTestReportDTO>> Handle(GetEmptyLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var report = await query.FindLastLabTestReportAsync(request.Date);

		if (report == null)
			return new LabTestReportDTO
			{
				Date = request.Date,
				SequentialNumber = 0
			};

		var availableNumber = report.SequentialNumber + 1;

		return new LabTestReportDTO
		{
			Date = request.Date,
			SequentialNumber = availableNumber
		};
	}
}