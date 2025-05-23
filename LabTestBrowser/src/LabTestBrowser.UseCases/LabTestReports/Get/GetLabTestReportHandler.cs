using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.LabTestReports.Get;

public class GetLabTestReportHandler(
	IReadRepository<LabTestReport> _repository,
	ILogger<GetLabTestReportHandler> _logger)
	: IQueryHandler<GetLabTestReportQuery, Result<LabTestReportDto>>
{
	public async Task<Result<LabTestReportDto>> Handle(GetLabTestReportQuery request, CancellationToken cancellationToken)
	{
		var accessionNumber = AccessionNumber.Create(request.OrderNumber, request.OrderDate);
		if (!accessionNumber.IsSuccess)
		{
			_logger.LogWarning("Invalid values for AccessionNumber: {sequenceNumber} {date}", request.OrderNumber, request.OrderDate);
			return Result.Invalid(accessionNumber.ValidationErrors);
		}

		var spec = new LabTestReportByAccessionNumberSpec(accessionNumber);
		var labTestReport = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (labTestReport == null)
			return new LabTestReportDto
			{
				OrderDate = request.OrderDate,
				OrderNumber = request.OrderNumber
			};

		var dto = labTestReport.ConvertToLabTestReportDto();

		return dto;
	}
}