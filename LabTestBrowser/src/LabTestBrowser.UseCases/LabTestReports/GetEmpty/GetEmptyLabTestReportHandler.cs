namespace LabTestBrowser.UseCases.LabTestReports.GetEmpty;

public class GetEmptyLabTestReportHandler(ILabTestReportQueryService _query)
	: IQueryHandler<GetEmptyLabTestReportQuery, Result<LabTestReportDTO>>
{
	public async Task<Result<LabTestReportDTO>> Handle(GetEmptyLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var report = await _query.FindLastLabTestReportAsync(request.Date);

		if (report == null)
			return new LabTestReportDTO
			{
				CollectionDate = request.Date,
				SpecimenSequentialNumber = 0
			};

		var availableNumber = report.SpecimenSequentialNumber + 1;

		return new LabTestReportDTO
		{
			CollectionDate = request.Date,
			SpecimenSequentialNumber = availableNumber
		};
	}
}