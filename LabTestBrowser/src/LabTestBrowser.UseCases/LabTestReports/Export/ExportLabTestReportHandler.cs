namespace LabTestBrowser.UseCases.LabTestReports.Export;

public class ExportLabTestReportHandler(ISpreadSheetExportService _exportService) : ICommandHandler<ExportLabTestReportCommand, Result>
{
	public async Task<Result> Handle(ExportLabTestReportCommand request, CancellationToken cancellationToken)
	{
		//TODO: Exception handling location

		var exportTasks = request.LabTestReportTemplateIds
			.Select(templateId => _exportService.Export(templateId, request.LabTestReportId));
		
		await Task.WhenAll(exportTasks);
		
		return Result.Success();
	}
}