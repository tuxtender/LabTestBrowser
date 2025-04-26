using System.Text.RegularExpressions;
using LabTestBrowser.UseCases.LabTestReports;

namespace LabTestBrowser.Infrastructure.Export;

public class ExportFileNamingService : IExportFileNamingService
{
	//TODO: Move to resources. Get with service or IOptions
	private const string DefaultPathTemplate  = "./Лабораторные отчеты/{{DATE}}/Отчет № {{SAMPLE}} от {{DATE}}";

	private readonly ITextTemplateEngine _textTemplateEngine;
	private readonly ExportSettings _exportSettings;
	private readonly ILogger<ExportFileNamingService> _logger;

	public ExportFileNamingService(IOptions<ExportSettings> exportSettings,
		ILogger<ExportFileNamingService> logger, ITextTemplateEngine textTemplateEngine)
	{
		_exportSettings = exportSettings.Value;
		_logger = logger;
		_textTemplateEngine = textTemplateEngine;
	}

	public Task<string> GetExportPathAsync(Dictionary<string, string> tokens, string fileExtension)
	{
		var pathTemplate = Path.Combine(_exportSettings.Directory, _exportSettings.Filename);
		var fullPath = Path.GetFullPath(pathTemplate);
		var rootPath = Path.GetPathRoot(fullPath) ?? string.Empty;
		var pathItems = Path.GetRelativePath(rootPath, fullPath)
			.Split(Path.DirectorySeparatorChar)
			.Select(pathItem => _textTemplateEngine.Render(pathItem, tokens))
			.Select(Sanitize)
			.Select(CollapseWhitespace);

		var path = Path.Combine([rootPath, ..pathItems]);
		var filename = Path.GetFileName(path);
		path = Path.ChangeExtension(path, fileExtension);

		if (!string.IsNullOrEmpty(filename))
			return Task.FromResult(path);

		var defaultPath = Path.ChangeExtension(DefaultPathTemplate, fileExtension);
		defaultPath = _textTemplateEngine.Render(defaultPath, tokens);
		defaultPath = Path.GetFullPath(defaultPath);
		_logger.LogWarning("Unsupported file system path characters. Default template applied: {defaultPath}", defaultPath);

		return Task.FromResult(defaultPath);
	}

	private static string Sanitize(string path)
	{
		var normalizedPathChars = path.Split(Path.GetInvalidFileNameChars());
		var normalizedPath = string.Concat(normalizedPathChars);
		var trimmedPath = normalizedPath
			.TrimStart(' ')
			.TrimEnd(' ', '.');

		return trimmedPath;
	}

	private static string CollapseWhitespace(string filename) => Regex.Replace(filename, @"\s\s+", " ");
}