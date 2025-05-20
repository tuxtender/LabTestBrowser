using LabTestBrowser.Infrastructure.Export;
using LabTestBrowser.Infrastructure.Export.PathSanitizer;
using LabTestBrowser.Infrastructure.Templating.Engines;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace LabTestBrowser.UnitTests.Infrastructure.Export;

public class ExportFileNamingServiceTests
{
	private readonly ITextTemplateEngine _textTemplateEngine = new TextTemplateEngine(NullLogger<TextTemplateEngine>.Instance);
	private readonly IDefaultPathProvider _defaultPathProvider = new StubDefaultPathProvider(string.Empty);
	private readonly IBasePathProvider _basePathProvider = new BasePathProvider();
	private readonly IPathSanitizer _pathSanitizer = new WindowsPathSanitizer();
	private readonly ILogger<ExportFileNamingService> _logger = NullLogger<ExportFileNamingService>.Instance;

	private static IEnumerable<string> InvalidFileNameCharStrings => Path.GetInvalidFileNameChars().Select(c => c.ToString());
	private static IEnumerable<string> InvalidPathCharStrings => Path.GetInvalidPathChars().Select(c => c.ToString());

	[Fact]
	public async Task GetExportPathAsync_ValidTokensAndTemplate_ReturnsCorrectPath()
	{
		var expectedRelativePath = "exports/01.01.2000/report_test #1";
		var expected = Path.GetFullPath(expectedRelativePath);
		var tokens = new Dictionary<string, string>
		{
			["DATE"] = "01.01.2000",
			["REPORT_NAME"] = "test #1"
		};
		var exportOptions = Options.Create(new ExportOptions
		{
			Directory = "exports/{{DATE}}",
			Filename = "report_{{REPORT_NAME}}"
		});
		var namingService =
			new ExportFileNamingService(_pathSanitizer, _textTemplateEngine, _defaultPathProvider,
				exportOptions, _basePathProvider, _logger);

		var path = await namingService.GetExportPathAsync(tokens, string.Empty);

		path.Should().Be(expected);
	}

	[Fact]
	public async Task GetExportPathAsync_WhenTokenContainsInvalidCharacters_ReturnsSanitizedPath()
	{
		var expectedTemplate = "exports/01.01.2000/report-";
		var expected = Path.GetFullPath(expectedTemplate, _basePathProvider.GetBasePath());
		var tokens = new Dictionary<string, string>
		{
			["INVALID_PATH_CHARS_TOKEN"] = $"{string.Concat(InvalidPathCharStrings)}01.01.2000",
			["INVALID_FILENAME_CHARS_TOKEN"] = $"report-{string.Concat(InvalidFileNameCharStrings)}"
		};
		var exportOptions = Options.Create(new ExportOptions
		{
			Directory = "exports/{{INVALID_PATH_CHARS_TOKEN}}",
			Filename = "{{INVALID_FILENAME_CHARS_TOKEN}}"
		});
		var namingService =
			new ExportFileNamingService(_pathSanitizer, _textTemplateEngine, _defaultPathProvider,
				exportOptions, _basePathProvider, _logger);

		var path = await namingService.GetExportPathAsync(tokens, string.Empty);

		path.Should().Be(expected);
		Path.GetFileName(path).Should().NotContainAny(InvalidFileNameCharStrings);
		Path.GetDirectoryName(path).Should().NotContainAny(InvalidPathCharStrings);
	}

	[Fact]
	public async Task GetExportPathAsync_WhenEmptyAfterRender_ReturnsFallback()
	{
		var fallbackRelativePath = "./Reports/Test";
		var expected = Path.GetFullPath(fallbackRelativePath, _basePathProvider.GetBasePath());
		var tokens = new Dictionary<string, string>
		{
			["EMPTY_TOKEN"] = ""
		};
		var exportOptions = Options.Create(new ExportOptions
		{
			Directory = "{{EMPTY_TOKEN}}",
			Filename = "{{EMPTY_TOKEN}}"
		});
		var defaultPathProvider = new StubDefaultPathProvider(fallbackRelativePath);
		var namingService =
			new ExportFileNamingService(_pathSanitizer, _textTemplateEngine, defaultPathProvider,
				exportOptions, _basePathProvider, _logger);

		var path = await namingService.GetExportPathAsync(tokens, string.Empty);

		path.Should().Be(expected);
	}

	[Fact]
	public async Task GetExportPathAsync_WhenEmptyAfterRender_ReturnsTemplatedFallback()
	{
		var tokens = new Dictionary<string, string>
		{
			["TOKEN0"] = "A",
			["TOKEN1"] = "B",
			["EMPTY_TOKEN"] = ""
		};
		var exportOptions = Options.Create(new ExportOptions
		{
			Directory = "{{EMPTY_TOKEN}}",
			Filename = "{{EMPTY_TOKEN}}"
		});
		var templatedFallbackRelativePath = "./Reports/{{TOKEN0}}/Test # {{TOKEN1}}";
		var fallbackPath = _textTemplateEngine.Render(templatedFallbackRelativePath, tokens);
		var expected = Path.GetFullPath(fallbackPath, _basePathProvider.GetBasePath());
		var defaultPathProvider = new StubDefaultPathProvider(templatedFallbackRelativePath);
		var namingService =
			new ExportFileNamingService(_pathSanitizer, _textTemplateEngine, defaultPathProvider,
				exportOptions, _basePathProvider, _logger);

		var path = await namingService.GetExportPathAsync(tokens, string.Empty);

		path.Should().Be(expected);
	}

	[Fact]
	public async Task GetExportPathAsync_WhenPathContainsDotSegments_KeepsThemIntactAndSanitizesOthers()
	{
		var expectedRelativePath = ".././../a/b/file-";
		var expected = Path.GetFullPath(expectedRelativePath, _basePathProvider.GetBasePath());
		var tokens = new Dictionary<string, string>
		{
			["TOKEN0"] = " . ",
			["TOKEN1"] = ".. ..",
		};
		var exportOptions = Options.Create(new ExportOptions
		{
			Directory = ".././../a/{{TOKEN0}}/b/{{TOKEN1}}",
			Filename = "file-{{TOKEN1}}"
		});
		var namingService =
			new ExportFileNamingService(_pathSanitizer, _textTemplateEngine, _defaultPathProvider,
				exportOptions, _basePathProvider, _logger);

		var path = await namingService.GetExportPathAsync(tokens, string.Empty);

		path.Should().Be(expected);
	}

	[Fact]
	public async Task GetExportPathAsync_WhenPathComponentContainsMultiWhitespace_CollapsesWhitespace()
	{
		var expectedRelativePath = "/a b c d/aA bB";
		var expected = Path.GetFullPath(expectedRelativePath, _basePathProvider.GetBasePath());
		var tokens = new Dictionary<string, string>
		{
			["TOKEN0"] = " a    b c  d ",
			["TOKEN1"] = " aA    bB ",
		};
		var exportOptions = Options.Create(new ExportOptions
		{
			Directory = "/{{TOKEN0}}",
			Filename = "{{TOKEN1}}"
		});
		var namingService =
			new ExportFileNamingService(_pathSanitizer, _textTemplateEngine, _defaultPathProvider,
				exportOptions, _basePathProvider, _logger);

		var path = await namingService.GetExportPathAsync(tokens, string.Empty);

		path.Should().Be(expected);
	}
}