using System.Text.RegularExpressions;
using LabTestBrowser.UseCases.Export;
using LabTestBrowser.UseCases.LabTestReports;

namespace LabTestBrowser.Infrastructure.Export;

public class ExportFileNamingService : IExportFileNamingService
{
	private readonly ITextTemplateEngine _textTemplateEngine;
	private readonly IDefaultPathProvider _defaultPathProvider;
	private readonly ExportSettings _exportSettings;
	private readonly ILogger<ExportFileNamingService> _logger;

	public ExportFileNamingService(ITextTemplateEngine textTemplateEngine, IDefaultPathProvider defaultPathProvider,
		IOptions<ExportSettings> exportSettings, ILogger<ExportFileNamingService> logger)
	{
		_textTemplateEngine = textTemplateEngine;
		_defaultPathProvider = defaultPathProvider;
		_exportSettings = exportSettings.Value;
		_logger = logger;
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

		var defaultPathTemplate = _defaultPathProvider.GetDefaultPath();
		var defaultPath = Path.ChangeExtension(defaultPathTemplate, fileExtension);
		defaultPath = _textTemplateEngine.Render(defaultPath, tokens);
		defaultPath = Path.GetFullPath(defaultPath);
		_logger.LogWarning("Unsupported file system path characters. Default template applied: {defaultPath}", defaultPathTemplate);

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