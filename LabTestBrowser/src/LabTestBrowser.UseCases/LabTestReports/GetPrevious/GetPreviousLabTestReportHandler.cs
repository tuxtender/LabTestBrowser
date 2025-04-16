using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReports.GetPrevious;

public class GetPreviousLabTestReportHandler(ILabTestReportQueryService _query, IReadRepository<LabTestReport> _repository)
	: IQueryHandler<GetPreviousLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetPreviousLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var lastLabTestReportDto = await _query.FindLastLabTestReportAsync(request.Date);

		if (lastLabTestReportDto == null)
			return new LabTestReportDto
			{
				SpecimenSequentialNumber = 1,
				Date = request.Date
			};

		var spec = new LabTestReportBySpecimenSpec(request.Specimen, request.Date);
		var labTestReport = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (labTestReport == null)
			return lastLabTestReportDto;

		var previousLabTestReportDto =
			await _query.FindPreviousLabTestReportAsync(labTestReport.Specimen.SequentialNumber, labTestReport.Specimen.ObservationDate);

		return previousLabTestReportDto ?? labTestReport.ConvertToLabTestReportDto();
	}
}