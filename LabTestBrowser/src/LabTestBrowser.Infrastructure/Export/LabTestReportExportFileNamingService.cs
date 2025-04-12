using System.Text.RegularExpressions;
using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.LabTestReports.Export;
using LabTestBrowser.UseCases.LabTestReportTemplates;

namespace LabTestBrowser.Infrastructure.Export;

public class LabTestReportExportFileNamingService : ILabTestReportExportFileNamingService
{
	private readonly IRepository<LabTestReport> _reportRepository;
	private readonly IRepository<CompleteBloodCount> _cbcRepository;
	private readonly ILabTestReportTemplateQueryService _templateQueryService;
	private readonly ITextTemplateEngine _textTemplateEngine;
	private readonly IOptions<ExportSettings> _exportSettings;
	private readonly ILogger<LabTestReportExportFileNamingService> _logger;

	public LabTestReportExportFileNamingService(IRepository<LabTestReport> reportRepository,
		IRepository<CompleteBloodCount> cbcRepository,
		ILabTestReportTemplateQueryService templateQueryService,
		IOptions<ExportSettings> exportSettings,
		ILogger<LabTestReportExportFileNamingService> logger, ITextTemplateEngine textTemplateEngine)
	{
		_reportRepository = reportRepository;
		_cbcRepository = cbcRepository;
		_templateQueryService = templateQueryService;
		_exportSettings = exportSettings;
		_logger = logger;
		_textTemplateEngine = textTemplateEngine;
	}

	public async Task<string> GetExportFilenameAsync(int labTestReportId, int labTestReportTemplateId)
	{
		var reportTemplate = await _templateQueryService.FindById(labTestReportTemplateId);

		if (reportTemplate == null)
			throw new Exception("Template not found");

		var templatePath = reportTemplate.Path;
		var fileInfo = new FileInfo(templatePath);

		if (!fileInfo.Exists)
		{
			throw new FileNotFoundException($"Template {fileInfo.FullName} doesn't exist");
		}

		_logger.LogInformation("Exporting lab test report template {templatePath}", templatePath);

		var report = await _reportRepository.GetByIdAsync(labTestReportId);
		CompleteBloodCount? cbc = null;

		if (report!.CompleteBloodCountId.HasValue)
			cbc = await _cbcRepository.GetByIdAsync(report.CompleteBloodCountId.Value);

		var tokens = new LabTestReportTokens(report, cbc);

		var tokenToValueMap = tokens.Tokens.ToDictionary(token => token.GetName(), token => token.GetValue());

		var filenameTemplate = _exportSettings.Value.Filename;
		var directoryTemplate = _exportSettings.Value.Directory;
		var fileExtension = Path.GetExtension(reportTemplate.Path);

		var pathTemplate = Path.Combine(directoryTemplate, filenameTemplate);
		var fullPath = Path.GetFullPath(pathTemplate);
		var rootPath = Path.GetPathRoot(fullPath) ?? string.Empty;
		var pathItems = Path.GetRelativePath(rootPath, fullPath)
			.Split(Path.DirectorySeparatorChar)
			.Select(pathItem => _textTemplateEngine.Render(pathItem, tokenToValueMap))
			.Select(Sanitize)
			.Select(CollapseWhitespace);

		var path = Path.Combine([rootPath, ..pathItems]);
		path = Path.ChangeExtension(path, fileExtension);

		if (!string.IsNullOrEmpty(path))
			return path;

		var defaultPath = $"/Лабораторные отчеты/Отчет № {labTestReportId} от {DateTime.Now:dd.MM.yyyy hh.mm.ss}";
		defaultPath = Path.ChangeExtension(defaultPath, fileExtension);
		_logger.LogWarning("Unsupported file system path characters. Default name applied: {defaultPath}", defaultPath);

		return defaultPath;
	}

	private string Sanitize(string path)
	{
		var normalizedPathChars = path.Split(Path.GetInvalidFileNameChars());
		var normalizedPath = string.Concat(normalizedPathChars);
		var trimmedPath = normalizedPath
			.TrimStart(' ')
			.TrimEnd(' ', '.');

		return trimmedPath;
	}

	private string CollapseWhitespace(string filename) => Regex.Replace(filename, @"\s\s+", " ");
}