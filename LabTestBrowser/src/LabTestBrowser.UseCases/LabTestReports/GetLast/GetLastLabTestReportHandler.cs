namespace LabTestBrowser.UseCases.LabTestReports.GetLast;

public class GetLastLabTestReportHandler(ILabTestReportQueryService _query)
	: IQueryHandler<GetLastLabTestReportQuery, Result<LabTestReportDTO>>
{
	public async Task<Result<LabTestReportDTO>> Handle(GetLastLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var labTestReport = await _query.FindLastLabTestReportAsync(request.Date);

		if (labTestReport == null)
			return new LabTestReportDTO
			{
				CollectionDate = request.Date,
				SpecimenSequentialNumber = 1
			};

		return labTestReport;
	}
}