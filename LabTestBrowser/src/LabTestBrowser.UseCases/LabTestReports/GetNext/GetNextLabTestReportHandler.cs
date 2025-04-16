using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReports.GetNext;

public class GetNextLabTestReportHandler(ILabTestReportQueryService _query, IReadRepository<LabTestReport> _repository)
	: IQueryHandler<GetNextLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetNextLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var spec = new LabTestReportBySpecimenSpec(request.Specimen, request.Date);
		var labTestReport = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (labTestReport == null)
			return new LabTestReportDto
			{
				SpecimenSequentialNumber = request.Specimen,
				Date = request.Date,
			};

		var nextLabTestReport =
			await _query.FindNextLabTestReportAsync(labTestReport.Specimen.SequentialNumber, labTestReport.Specimen.ObservationDate);

		return nextLabTestReport ?? labTestReport.ConvertToLabTestReportDto();
	}
}