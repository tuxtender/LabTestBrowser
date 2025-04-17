using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReports.Get;

public class GetLabTestReportHandler(IReadRepository<LabTestReport> _repository)
	: IQueryHandler<GetLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var accessionNumber = AccessionNumber.Create(request.SequenceNumber, request.Date);

		if (!accessionNumber.IsSuccess)
			return Result.Error();

		var spec = new LabTestReportByAccessionNumberSpec(accessionNumber);
		var labTestReport = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (labTestReport == null)
			return new LabTestReportDto
			{
				Date = request.Date,
				SequenceNumber = request.SequenceNumber
			};

		var dto = labTestReport.ConvertToLabTestReportDto();

		return dto;
	}
}