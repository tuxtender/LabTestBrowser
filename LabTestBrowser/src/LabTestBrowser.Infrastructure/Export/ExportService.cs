using Ardalis.Result;
using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.LabTestReports.Export;
using LabTestBrowser.UseCases.LabTestReportTemplates;

namespace LabTestBrowser.Infrastructure.Export;

public class ExportService : IExportService
{
	private readonly IRepository<LabTestReport> _reportRepository;
	private readonly ILabTestReportTemplateQueryService _templateQueryService;
	private readonly IRepository<CompleteBloodCount> _cbcRepository;
	private readonly ILabTestReportExportFileNamingService _exportFileNamingService;
	private readonly IFileTemplateEngine _templateEngine;
	private readonly ILogger<ExportService> _logger;

	public ExportService(IRepository<LabTestReport> reportRepository,
		ILabTestReportTemplateQueryService templateQueryService,
		IRepository<CompleteBloodCount> cbcRepository,
		ILabTestReportExportFileNamingService exportFileNamingService,
		IFileTemplateEngine templateEngine,
		ILogger<ExportService> logger)
	{
		_reportRepository = reportRepository;
		_templateQueryService = templateQueryService;
		_cbcRepository = cbcRepository;
		_exportFileNamingService = exportFileNamingService;
		_templateEngine = templateEngine;
		_logger = logger;
	}

	public async Task<Result> ExportAsync(int labTestReportId, int labTestReportTemplateId)
	{
		var reportTemplate = await _templateQueryService.FindById(labTestReportTemplateId);

		if (reportTemplate == null)
		{
			// throw new FileNotFoundException("Template not found");
			return Result.NotFound();
		}

		var templatePath = Path.GetFullPath(reportTemplate.Path);

		if (!File.Exists(templatePath))
			throw new FileNotFoundException($"Template {templatePath} doesn't exist");

		var report = await _reportRepository.GetByIdAsync(labTestReportId);
		CompleteBloodCount? cbc = null;

		if (report!.CompleteBloodCountId.HasValue)
			cbc = await _cbcRepository.GetByIdAsync(report.CompleteBloodCountId.Value);

		_logger.LogInformation("Exporting report id: {report.Id} to template: {templatePath}", templatePath, report.Id);

		await using var fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

		var templateExtension = Path.GetExtension(reportTemplate.Path);

		//TODO: refactor LabTestReportTokens
		var reportTokens = new LabTestReportTokens(report, cbc);
		var tokens = reportTokens.Tokens.ToDictionary(token => token.GetName(), token => token.GetValue());

		MemoryStream memoryStream = new MemoryStream();

		if (templateExtension == ".xlsx")
			memoryStream = await _templateEngine.RenderAsync(fileStream, tokens);

		//TODO: Separate filename and directory creation
		var exportPath = await _exportFileNamingService.GetExportFilenameAsync(labTestReportTemplateId, labTestReportTemplateId);

		var directory = Path.GetDirectoryName(exportPath) ?? string.Empty;
		Directory.CreateDirectory(directory);

		await using var fs = new FileStream(exportPath, FileMode.Create);
		await memoryStream.CopyToAsync(fs);
		await memoryStream.DisposeAsync();

		_logger.LogInformation("Report exported: {exportPath}", exportPath);

		return Result.Success();
	}
}