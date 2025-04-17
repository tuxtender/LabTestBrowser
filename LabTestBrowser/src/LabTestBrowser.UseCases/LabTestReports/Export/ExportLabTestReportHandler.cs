using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UseCases.LabTestReports.Export;

public class ExportLabTestReportHandler(IExportService _exportService, IReadRepository<LabTestReport> _repository)
	: ICommandHandler<ExportLabTestReportCommand, Result>
{
	public async Task<Result> Handle(ExportLabTestReportCommand request, CancellationToken cancellationToken)
	{
		//TODO: Exception handling location

		if (!request.LabTestReportId.HasValue)
			return Result.Error();

		var report = await _repository.GetByIdAsync(request.LabTestReportId.Value, cancellationToken);

		if (report == null)
			return Result.NotFound();

		var exportTasks = request.LabTestReportTemplateIds
			.Select(templateId => _exportService.ExportAsync(report.Id, templateId));

		await Task.WhenAll(exportTasks);

		return Result.Success();
	}
}