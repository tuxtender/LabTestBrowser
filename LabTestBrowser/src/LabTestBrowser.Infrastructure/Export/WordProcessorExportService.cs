using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.LabTestReports.Export;
using NPOI.XWPF.UserModel;

namespace LabTestBrowser.Infrastructure.Export;

public class WordProcessorExportService : IWordProcessorExportService
{
	private readonly IRepository<LabTestReport> _reportRepository;
	private readonly IRepository<CompleteBloodCount> _cbcRepository;
	private readonly ILogger<WordProcessorExportService> _logger;

	public WordProcessorExportService(IRepository<LabTestReport> reportRepository,
		IRepository<CompleteBloodCount> cbcRepository, ILogger<WordProcessorExportService> logger)
	{
		_reportRepository = reportRepository;
		_cbcRepository = cbcRepository;
		_logger = logger;
	}

	public async Task Export(LabTestReportTemplate reportTemplate, int labTestReportId)
	{
		// var exportDirectory = @"C:\Users\tuxtender\RiderProjects\BloodMoney\TestResults\Word";
		// var exportFileName = Path.Combine(exportDirectory, "TestResults.docx");

		var templatePath = reportTemplate.Path;

		_logger.LogDebug("Template opening: {templatePath}", templatePath);

		var report = await _reportRepository.GetByIdAsync(labTestReportId);
		CompleteBloodCount? cbc = null;

		if (report!.CompleteBloodCountId.HasValue)
			cbc = await _cbcRepository.GetByIdAsync(report.CompleteBloodCountId.Value);

		var tokens = new LabTestReportTokens(report, cbc);

		await using var fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		var doc = new XWPFDocument(fileStream);

		foreach (var token in tokens.Tokens)
		{
			var value = token.GetValue();
			doc.FindAndReplaceText($"{{{{{token}}}}}", value);
		}

		var memoryStream = new MemoryStream();
		doc.Write(memoryStream);
		doc.Close();
		memoryStream.Position = 0;

		var exportFilenamePath = string.Empty;
		_logger.LogDebug("Template exported: {exportPath}", string.Empty);

		await using var fs = new FileStream(exportFilenamePath, FileMode.Create);
		await memoryStream.CopyToAsync(fs);
	}
}