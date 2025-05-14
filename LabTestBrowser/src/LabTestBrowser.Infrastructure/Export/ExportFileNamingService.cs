using System.Text.RegularExpressions;
using LabTestBrowser.Infrastructure.Templating.Engines;
using LabTestBrowser.UseCases.Export;

namespace LabTestBrowser.Infrastructure.Export;

public class ExportFileNamingService : IExportFileNamingService
{
	private readonly ITextTemplateEngine _textTemplateEngine;
	private readonly IDefaultPathProvider _defaultPathProvider;
	private readonly ExportOptions _settings;
	private readonly ILogger<ExportFileNamingService> _logger;

	public ExportFileNamingService(ITextTemplateEngine textTemplateEngine, IDefaultPathProvider defaultPathProvider,
		IOptions<ExportOptions> exportOptions, ILogger<ExportFileNamingService> logger)
	{
		_textTemplateEngine = textTemplateEngine;
		_defaultPathProvider = defaultPathProvider;
		_settings = exportOptions.Value;
		_logger = logger;
	}

	public Task<string> GetExportPathAsync(IReadOnlyDictionary<string, string> tokens, string fileExtension)
	{
		var pathTemplate = Path.Combine(_settings.Directory, _settings.Filename);
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
		_logger.LogWarning("Unsupported file system path characters. Default template applied: {DefaultPath}", defaultPathTemplate);

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