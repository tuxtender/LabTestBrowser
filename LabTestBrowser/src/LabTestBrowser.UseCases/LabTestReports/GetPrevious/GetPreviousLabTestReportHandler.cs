using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReports.GetPrevious;

public class GetPreviousLabTestReportHandler(ILabTestReportQueryService _query, IReadRepository<LabTestReport> _repository)
	: IQueryHandler<GetPreviousLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetPreviousLabTestReportQuery request, CancellationToken cancellationToken)
	{
		//TODO: Remove nesting
		var spec = new LabTestReportBySpecimenSpec(request.Specimen, request.Date);
		var labTestReport = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (labTestReport == null)
		{
			var defaultReport = new LabTestReportDto
			{
				SpecimenSequentialNumber = request.Specimen,
				Date = request.Date,
			};
			var lastLabTestReport = await _query.FindLastLabTestReportAsync(request.Date);

			return lastLabTestReport ?? defaultReport;
		}

		var previousLabTestReport =
			await _query.FindPreviousLabTestReportAsync(labTestReport.Specimen.SequentialNumber, labTestReport.Specimen.Date);

		return previousLabTestReport ?? labTestReport.ConvertToLabTestReportDto();
	}
}