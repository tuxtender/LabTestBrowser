using Ardalis.Result;
using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.CompleteBloodCountAggregate.Specifications;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UseCases.Export;
using LabTestBrowser.UseCases.LabTestReportTemplates;

namespace LabTestBrowser.Infrastructure.Export;

public class ExportService : IExportService
{
	private readonly IRepository<LabTestReport> _reportRepository;
	private readonly ILabTestReportTemplateQueryService _templateQueryService;
	private readonly IRepository<CompleteBloodCount> _cbcRepository;
	private readonly IExportFileNamingService _exportFileNamingService;
	private readonly ITemplateEngineResolver _templateEngineResolver;
	private readonly ILogger<ExportService> _logger;

	public ExportService(IRepository<LabTestReport> reportRepository,
		ILabTestReportTemplateQueryService templateQueryService,
		IRepository<CompleteBloodCount> cbcRepository,
		IExportFileNamingService exportFileNamingService,
		ITemplateEngineResolver templateEngineResolver,
		ILogger<ExportService> logger)
	{
		_reportRepository = reportRepository;
		_templateQueryService = templateQueryService;
		_cbcRepository = cbcRepository;
		_exportFileNamingService = exportFileNamingService;
		_templateEngineResolver = templateEngineResolver;
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

		if (report == null)
			return Result.NotFound();

		var spec = new CompleteBloodCountByAccessionNumberSpec(report.AccessionNumber);
		var cbc = await _cbcRepository.FirstOrDefaultAsync(spec);

		_logger.LogInformation("Exporting report id: {report.Id} to template: {templatePath}", templatePath, report.Id);

		await using var fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

		//TODO: refactor LabTestReportTokens
		var reportTokens = new LabTestReportTokens(report, cbc, reportTemplate.Title);
		var tokens = reportTokens.Tokens.ToDictionary(token => token.GetName(), token => token.GetValue());

		var templateEngine = _templateEngineResolver.ResolveByFileFormat(reportTemplate.FileFormat);
		await using var memoryStream = await templateEngine.RenderAsync(fileStream, tokens);
		var exportPath = await _exportFileNamingService.GetExportPathAsync(tokens, reportTemplate.FileExtension);
		//TODO: IOException treatment
		var directory = Path.GetDirectoryName(exportPath) ?? string.Empty;
		Directory.CreateDirectory(directory);

		await using var fs = new FileStream(exportPath, FileMode.Create);
		await memoryStream.CopyToAsync(fs);

		_logger.LogInformation("Report exported: {exportPath}", exportPath);

		return Result.Success();
	}
}