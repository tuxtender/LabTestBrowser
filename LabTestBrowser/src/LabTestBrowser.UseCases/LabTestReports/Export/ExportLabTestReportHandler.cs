using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReports.Export;

public class ExportLabTestReportHandler(IExportService _exportService, IReadRepository<LabTestReport> _repository)
	: ICommandHandler<ExportLabTestReportCommand, Result>
{
	public async Task<Result> Handle(ExportLabTestReportCommand request, CancellationToken cancellationToken)
	{
		//TODO: Exception handling location

		var spec = new LabTestReportBySpecimenSpec(request.Specimen, request.Date);
		var report = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (report == null)
			return Result.NotFound();

		var exportTasks = request.LabTestReportTemplateIds
			.Select(templateId => _exportService.ExportAsync(report.Id, templateId));

		await Task.WhenAll(exportTasks);

		return Result.Success();
	}
}