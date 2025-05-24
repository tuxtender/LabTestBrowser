using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.CompleteBloodCountAggregate.Specifications;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportTemplateAggregate;
using LabTestBrowser.UseCases.Export.Exceptions;
using LabTestBrowser.UseCases.LabTestReportTemplates;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.Export;

public class ExportLabTestReportHandler : ICommandHandler<ExportLabTestReportCommand, Result>
{
	private readonly IExportService _exportService;
	private readonly IErrorLocalizationService _errorLocalizer;
	private readonly ILogger<ExportLabTestReportHandler> _logger;

	private readonly IRepository<LabTestReport> _reportRepository;
	private readonly ILabTestReportTemplateQueryService _templateQueryService;
	private readonly IRepository<CompleteBloodCount> _cbcRepository;

	public ExportLabTestReportHandler(IExportService exportService,
		IErrorLocalizationService errorLocalizer,
		ILogger<ExportLabTestReportHandler> logger, IRepository<LabTestReport> reportRepository,
		ILabTestReportTemplateQueryService templateQueryService, IRepository<CompleteBloodCount> cbcRepository,
		IExportFileNamingService exportFileNamingService)
	{
		_exportService = exportService;
		_errorLocalizer = errorLocalizer;
		_logger = logger;
		_reportRepository = reportRepository;
		_templateQueryService = templateQueryService;
		_cbcRepository = cbcRepository;
	}

	public async Task<Result> Handle(ExportLabTestReportCommand request, CancellationToken cancellationToken)
	{
		if (!request.LabTestReportId.HasValue)
			return Result.Invalid(new ValidationError(_errorLocalizer.LabTestReportNotSaved));

		var report = await _reportRepository.GetByIdAsync(request.LabTestReportId.Value, cancellationToken);
		if (report == null)
		{
			_logger.LogWarning("Missing required LabTestReport id: {LabTestReportId}", request.LabTestReportId.Value);
			return Result.CriticalError(_errorLocalizer.ExportFailed);
		}

		var spec = new CompleteBloodCountByAccessionNumberSpec(report.AccessionNumber);
		var completeBloodCount = await _cbcRepository.FirstOrDefaultAsync(spec, cancellationToken);

		var reportTemplateTasks = request.LabTestReportTemplateIds.Select(id => _templateQueryService.FindById(id));
		var reportTemplates = await Task.WhenAll(reportTemplateTasks);
		if (reportTemplates.Contains(null))
		{
			_logger.LogWarning("Some LabTestReportTemplate could not be found");
			return Result.CriticalError(_errorLocalizer.ExportFailed);
		}

		var exportTasks = reportTemplates
			.OfType<LabTestReportTemplate>()
			.Select(template => ExportAsync(template, report, completeBloodCount))
			.ToArray();

		var results = await Task.WhenAll(exportTasks);
		if (results.All(result => result.IsSuccess))
			return Result.Success();

		return Result.Error(_errorLocalizer.ExportFailed);
	}

	private async Task<Result> ExportAsync(LabTestReportTemplate template, LabTestReport report, CompleteBloodCount? completeBloodCount)
	{
		try
		{
			await _exportService.ExportAsync(template, report, completeBloodCount);
			return Result.Success();
		}
		catch (ExportFileAccessException ex)
		{
			_logger.LogWarning(ex, "Failed to access export file: '{ExportFilePath}'", ex.FilePath);
			return Result.Error();
		}

		catch (TemplateFileAccessException ex)
		{
			_logger.LogWarning(ex, "Failed to access template file: '{TemplateFilePath}'", ex.FilePath);
			return Result.Error();
		}
		catch (TemplateEngineNotFoundException)
		{
			_logger.LogError("Template engine isn't registered");
			return Result.Error();
		}
		catch (TemplateFileNotFoundException ex)
		{
			_logger.LogWarning(ex, "Failed to find template file: '{FilePath}'", ex.FilePath);
			return Result.Error();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unexpected error occured");
			return Result.Error();
		}
	}
}