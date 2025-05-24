using System.Text.RegularExpressions;
using LabTestBrowser.Infrastructure.Export.PathSanitizer;
using LabTestBrowser.Infrastructure.Templating.Engines;
using LabTestBrowser.UseCases.Export;

namespace LabTestBrowser.Infrastructure.Export;

public class ExportFileNamingService : IExportFileNamingService
{
	private readonly IPathSanitizer _pathSanitizer;
	private readonly ITextTemplateEngine _textTemplateEngine;
	private readonly IDefaultPathProvider _defaultPathProvider;
	private readonly IBasePathProvider _basePathProvider;
	private readonly ExportOptions _settings;
	private readonly ILogger<ExportFileNamingService> _logger;

	public ExportFileNamingService(IPathSanitizer pathSanitizer,
		ITextTemplateEngine textTemplateEngine, IDefaultPathProvider defaultPathProvider,
		IOptions<ExportOptions> exportOptions, IBasePathProvider basePathProvider, ILogger<ExportFileNamingService> logger)
	{
		_pathSanitizer = pathSanitizer;
		_textTemplateEngine = textTemplateEngine;
		_defaultPathProvider = defaultPathProvider;
		_basePathProvider = basePathProvider;
		_settings = exportOptions.Value;
		_logger = logger;
	}

	public Task<string> GetExportPathAsync(IReadOnlyDictionary<string, string> tokens, string fileExtension)
	{
		var normalizedAbsolutePath = Path.GetFullPath(_settings.Directory, _basePathProvider.GetBasePath());
		var pathComponents = Path.GetRelativePath(_basePathProvider.GetBasePath(), normalizedAbsolutePath)
			.Split(Path.DirectorySeparatorChar)
			.Select(pathComponent => _textTemplateEngine.Render(pathComponent, tokens))
			.Select(Sanitize)
			.Select(CollapseWhitespace)
			.Where(pathComponent => !string.IsNullOrEmpty(pathComponent));

		var directory = Path.Combine([..pathComponents]);
		if (string.IsNullOrEmpty(directory))
			directory = GetFallbackDirectory(tokens);

		var renderedFilename = _textTemplateEngine.Render(_settings.Filename, tokens);
		var sanitizedFilename = _pathSanitizer.Sanitize(renderedFilename);
		var filename = CollapseWhitespace(sanitizedFilename);
		if (string.IsNullOrEmpty(filename))
			filename = GetFallbackFilename(tokens);

		var relativePath = Path.Combine(directory, filename);
		relativePath = Path.ChangeExtension(relativePath, fileExtension);
		var absolutePath = Path.GetFullPath(relativePath, _basePathProvider.GetBasePath());

		return Task.FromResult(absolutePath);
	}

	private string Sanitize(string pathComponent)
	{
		if (pathComponent is "." or "..")
			return pathComponent;

		var sanitized = _pathSanitizer.Sanitize(pathComponent);

		return sanitized;
	}

	private string GetFallbackDirectory(IReadOnlyDictionary<string, string> tokens)
	{
		var templatedPath = _defaultPathProvider.GetDefaultPath();
		var templatedDirectory = Path.GetDirectoryName(templatedPath) ?? string.Empty;
		var directory = _textTemplateEngine.Render(templatedDirectory, tokens);
		_logger.LogWarning("Fallback directory applied: {FallbackExportDirectoryTemplate}", templatedDirectory);

		return directory;
	}

	private string GetFallbackFilename(IReadOnlyDictionary<string, string> tokens)
	{
		var templatedPath = _defaultPathProvider.GetDefaultPath();
		var templatedFilename = Path.GetFileName(templatedPath);
		var filename = _textTemplateEngine.Render(templatedFilename, tokens);
		_logger.LogWarning("Fallback filename applied: {FallbackExportFilenameTemplate}", templatedFilename);

		return filename;
	}

	private static string CollapseWhitespace(string filename) => Regex.Replace(filename, @"\s\s+", " ");
}