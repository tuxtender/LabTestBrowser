using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.LabTestReports.GetPrevious;

public class GetPreviousLabTestReportHandler(
	ILabTestReportQueryService _query,
	IReadRepository<LabTestReport> _repository,
	ILogger<GetPreviousLabTestReportHandler> _logger)
	: IQueryHandler<GetPreviousLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetPreviousLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var accessionNumber = AccessionNumber.Create(request.OrderNumber, request.OrderDate);
		if (!accessionNumber.IsSuccess)
		{
			_logger.LogWarning("Invalid values for AccessionNumber: {sequenceNumber} {date}", request.OrderNumber, request.OrderDate);
			return Result.Invalid(accessionNumber.ValidationErrors);
		}

		var lastLabTestReportDto = await _query.FindLastLabTestReportAsync(request.OrderDate);
		if (lastLabTestReportDto == null)
			return new LabTestReportDto
			{
				OrderNumber = 1,
				OrderDate = request.OrderDate
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