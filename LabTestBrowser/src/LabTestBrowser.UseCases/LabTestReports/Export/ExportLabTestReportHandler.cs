namespace LabTestBrowser.UseCases.LabTestReports.Export;

public class ExportLabTestReportHandler(IExportService _exportService) : ICommandHandler<ExportLabTestReportCommand, Result>
{
	public async Task<Result> Handle(ExportLabTestReportCommand request, CancellationToken cancellationToken)
	{
		//TODO: Exception handling location

		var exportTasks = request.LabTestReportTemplateIds
			.Select(templateId => _exportService.ExportAsync(request.LabTestReportId, templateId));
		
		await Task.WhenAll(exportTasks);
		
		return Result.Success();
	}
}