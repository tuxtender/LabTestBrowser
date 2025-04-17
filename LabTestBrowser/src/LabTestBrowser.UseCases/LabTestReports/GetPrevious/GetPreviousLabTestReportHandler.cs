using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReports.GetPrevious;

public class GetPreviousLabTestReportHandler(ILabTestReportQueryService _query, IReadRepository<LabTestReport> _repository)
	: IQueryHandler<GetPreviousLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetPreviousLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var accessionNumber = AccessionNumber.Create(request.SequenceNumber, request.Date);

		if (!accessionNumber.IsSuccess)
			return Result.Error();

		var lastLabTestReportDto = await _query.FindLastLabTestReportAsync(request.Date);

		if (lastLabTestReportDto == null)
			return new LabTestReportDto
			{
				SequenceNumber = 1,
				Date = request.Date
			};

		var spec = new LabTestReportByAccessionNumberSpec(accessionNumber);
		var labTestReport = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (labTestReport == null)
			return lastLabTestReportDto;

		var previousLabTestReportDto =
			await _query.FindPreviousLabTestReportAsync(labTestReport.AccessionNumber.SequenceNumber, labTestReport.AccessionNumber.Date);

		return previousLabTestReportDto ?? labTestReport.ConvertToLabTestReportDto();
	}
}