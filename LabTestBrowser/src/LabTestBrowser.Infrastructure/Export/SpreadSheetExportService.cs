using System.Text.RegularExpressions;
using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.LabTestReports.Export;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LabTestBrowser.Infrastructure.Export;

public class SpreadSheetExportService : ISpreadSheetExportService
{
	private readonly IRepository<LabTestReport> _reportRepository;
	private readonly IRepository<CompleteBloodCount> _cbcRepository;
	private readonly ILogger<SpreadSheetExportService> _logger;

	public SpreadSheetExportService(IRepository<LabTestReport> reportRepository,
		IRepository<CompleteBloodCount> cbcRepository,
		ILogger<SpreadSheetExportService> logger)
	{
		_reportRepository = reportRepository;
		_cbcRepository = cbcRepository;
		_logger = logger;
	}

	public async Task Export(LabTestReportTemplate reportTemplate, int labTestReportId)
	{
		var templatePath = reportTemplate.Path;

		_logger.LogDebug("Template opening: {templatePath}", templatePath);

		var report = await _reportRepository.GetByIdAsync(labTestReportId);
		CompleteBloodCount? cbc = null;

		if (report!.CompleteBloodCountId.HasValue)
			cbc = await _cbcRepository.GetByIdAsync(report.CompleteBloodCountId.Value);

		var tokens = new LabTestReportTokens(report, cbc);

		var tokenToValueMap = tokens.Tokens.ToDictionary(token => token.GetName(), token => token.GetValue());

		await using var fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		var workbook = new XSSFWorkbook(fileStream);
		var sheet = workbook.GetSheetAt(0);

		const string pattern = @"{{\S*}}";
		var regex = new Regex(pattern);
		
		foreach (IRow row in sheet)
		{
			foreach (var cell in row)
			{
				var cellValue = cell.ToString();

				if (cellValue == null)
					continue;

				if (!regex.IsMatch(cellValue))
					continue;

				var result = regex.Replace(cellValue, (match) =>
				{
					var token = match.Value[2..^2];

					if (tokenToValueMap.TryGetValue(token, out var value))
					{
						_logger.LogDebug("Replaced token: {token}, value : {value}", token, value);
						return value;
					}

					_logger.LogDebug("Skipped unregistered token: {token}", token);

					return string.Empty;
				});

				//TODO: Fix numeric cast
				if (float.TryParse(result, System.Globalization.CultureInfo.InvariantCulture, out var resultFloat))
					cell.SetCellValue(resultFloat);
				else
				{
					cell.SetCellValue(result);
				}
			}
		}

		var memoryStream = new MemoryStream();
		workbook.Write(memoryStream, leaveOpen: true);
		workbook.Close();
		memoryStream.Position = 0;

		var exportFilenamePath = string.Empty;
		_logger.LogDebug("Template exported: {exportPath}", string.Empty);

		await using var fs = new FileStream(exportFilenamePath, FileMode.Create);
		await memoryStream.CopyToAsync(fs);
	}
}