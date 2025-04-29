using LabTestBrowser.Core.LabTestReportAggregate;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.LabTestReports.Export;

public class ExportLabTestReportHandler(
	IExportService _exportService,
	IReadRepository<LabTestReport> _repository,
	IErrorLocalizationService _errorLocalizer,
	ILogger<ExportLabTestReportHandler> _logger)
	: ICommandHandler<ExportLabTestReportCommand, Result>
{
	public async Task<Result> Handle(ExportLabTestReportCommand request, CancellationToken cancellationToken)
	{
		//TODO: Exception handling location

		if (!request.LabTestReportId.HasValue)
			return Result.Invalid(new ValidationError(_errorLocalizer.GetLabTestReportRequired()));

		var report = await _repository.GetByIdAsync(request.LabTestReportId.Value, cancellationToken);
		if (report == null)
		{
			_logger.LogWarning("Missing required LabTestReport id: {completeBloodCountId} in database", request.LabTestReportId.Value);
			return Result.CriticalError(_errorLocalizer.GetExportFailed());
		}

		var exportTasks = request.LabTestReportTemplateIds
			.Select(templateId => _exportService.ExportAsync(report.Id, templateId));

		await Task.WhenAll(exportTasks);

		return Result.Success();
	}
}