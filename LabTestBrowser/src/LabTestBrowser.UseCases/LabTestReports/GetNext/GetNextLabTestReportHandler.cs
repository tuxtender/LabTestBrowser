using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReports.GetNext;

public class GetNextLabTestReportHandler(ILabTestReportQueryService _query, IReadRepository<LabTestReport> _repository)
	: IQueryHandler<GetNextLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetNextLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var accessionNumber = AccessionNumber.Create(request.OrderNumber, request.OrderDate);

		if (!accessionNumber.IsSuccess)
			return Result.Error();

		var spec = new LabTestReportByAccessionNumberSpec(accessionNumber);
		var labTestReport = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (labTestReport == null)
			return new LabTestReportDto
			{
				OrderNumber = request.OrderNumber,
				OrderDate = request.OrderDate,
			};

		var nextLabTestReport =
			await _query.FindNextLabTestReportAsync(labTestReport.AccessionNumber.SequenceNumber, labTestReport.AccessionNumber.Date);

		return nextLabTestReport ?? labTestReport.ConvertToLabTestReportDto();
	}
}