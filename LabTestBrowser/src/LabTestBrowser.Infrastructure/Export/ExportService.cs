using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportTemplateAggregate;
using LabTestBrowser.UseCases.Export;
using LabTestBrowser.UseCases.Export.Exceptions;

namespace LabTestBrowser.Infrastructure.Export;

public class ExportService : IExportService
{
	private readonly IExportFileNamingService _exportFileNamingService;
	private readonly ITemplateEngineResolver _templateEngineResolver;
	private readonly ILogger<ExportService> _logger;

	public ExportService(IExportFileNamingService exportFileNamingService,
		ITemplateEngineResolver templateEngineResolver,
		ILogger<ExportService> logger)
	{
		_exportFileNamingService = exportFileNamingService;
		_templateEngineResolver = templateEngineResolver;
		_logger = logger;
	}

	public async Task ExportAsync(LabTestReportTemplate reportTemplate, LabTestReport report, CompleteBloodCount? completeBloodCount)
	{
		_logger.LogInformation("Exporting template: {TemplateFilePath}", reportTemplate.Path);

		//TODO: refactor LabTestReportTokens
		var reportTokens = new LabTestReportTokens(report, completeBloodCount, reportTemplate.Title);
		var tokens = reportTokens.Tokens.ToDictionary(token => token.GetName(), token => token.GetValue());

		await using var memoryStream = await RenderTemplateAsync(reportTemplate, tokens);
		var exportPath = await _exportFileNamingService.GetExportPathAsync(tokens, reportTemplate.TemplateFileExtension.FileExtension);
		CreateExportDirectory(exportPath);
		await WriteToFileAsync(exportPath, memoryStream);

		_logger.LogInformation("Report exported: {ExportPath}", exportPath);
	}

	private static void CreateExportDirectory(string exportPath)
	{
		try
		{
			var directory = Path.GetDirectoryName(exportPath);
			if (!string.IsNullOrEmpty(directory))
				Directory.CreateDirectory(directory);
		}
		catch (Exception ex)
		{
			throw new ExportFileAccessException(exportPath, "Failed to create directory", ex);
		}
	}

	private async Task<MemoryStream> RenderTemplateAsync(LabTestReportTemplate reportTemplate, Dictionary<string, string> tokens)
	{
		try
		{
			await using var fileStream = new FileStream(reportTemplate.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			var templateEngine = _templateEngineResolver.ResolveByFileFormat(reportTemplate.TemplateFileExtension.FileFormat);
			var memoryStream = await templateEngine.RenderAsync(fileStream, tokens);
			return memoryStream;
		}
		catch (DirectoryNotFoundException ex)
		{
			throw new TemplateFileNotFoundException(reportTemplate.Path, "Template file not found", ex);
		}
		catch (FileNotFoundException ex)
		{
			throw new TemplateFileNotFoundException(reportTemplate.Path, "Template file not found", ex);
		}
		catch (UnauthorizedAccessException ex)
		{
			throw new TemplateFileAccessException(reportTemplate.Path,
				"Failed to access the template file due to insufficient permissions", ex);
		}
		catch (IOException ex)
		{
			throw new TemplateFileAccessException(reportTemplate.Path, "Access to the template file was denied",
				ex);
		}
	}

	private static async Task WriteToFileAsync(string exportPath, MemoryStream memoryStream)
	{
		try
		{
			await using var fs = new FileStream(exportPath, FileMode.Create);
			await memoryStream.CopyToAsync(fs);
		}
		catch (IOException ex)
		{
			throw new ExportFileAccessException(exportPath, "Access to the export file was denied", ex);
		}
		catch (UnauthorizedAccessException ex)
		{
			throw new ExportFileAccessException(exportPath,
				$"Failed to access the export file due to insufficient permissions", ex);
		}
	}
}