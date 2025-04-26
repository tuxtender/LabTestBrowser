using LabTestBrowser.Core.LabTestReportAggregate;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.LabTestReports.Export;

public class ExportLabTestReportHandler(
	IExportService _exportService,
	IReadRepository<LabTestReport> _repository,
	ILogger<ExportLabTestReportHandler> _logger)
	: ICommandHandler<ExportLabTestReportCommand, Result>
{
	public async Task<Result> Handle(ExportLabTestReportCommand request, CancellationToken cancellationToken)
	{
		//TODO: Exception handling location

		if (!request.LabTestReportId.HasValue)
			return Result.Invalid(new ValidationError("ValidationError.LabTestReportRequired", "No test selected"));

		var report = await _repository.GetByIdAsync(request.LabTestReportId.Value, cancellationToken);
		if (report == null)
		{
			_logger.LogWarning("Missing required LabTestReport id: {completeBloodCountId} in database", request.LabTestReportId.Value);
			return Result.CriticalError("ErrorMessage.ExportFailed");
		}

		var exportTasks = request.LabTestReportTemplateIds
			.Select(templateId => _exportService.ExportAsync(report.Id, templateId));

		await Task.WhenAll(exportTasks);

		return Result.SuccessWithMessage("SuccessMessage.ExportLabTestReport");
	}
}