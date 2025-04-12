using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LabTestBrowser.Infrastructure.Export;

public class ExcelTemplateEngine : IFileTemplateEngine
{
	private readonly ITextTemplateEngine _templateEngine;
	private readonly ILogger<ExcelTemplateEngine> _logger;

	public ExcelTemplateEngine(ITextTemplateEngine templateEngine,
		ILogger<ExcelTemplateEngine> logger)
	{
		_templateEngine = templateEngine;
		_logger = logger;
	}

	public Task<MemoryStream> RenderAsync(FileStream fileStream, Dictionary<string, string> tokens)
	{
		var workbook = new XSSFWorkbook(fileStream);
		var sheet = workbook.GetSheetAt(0);

		foreach (IRow row in sheet)
		{
			foreach (var cell in row)
			{
				var cellValue = cell.ToString();

				if (cellValue == null)
					continue;

				var value = _templateEngine.Render(cellValue, tokens);

				//TODO: Fix numeric cast
				if (float.TryParse(value, System.Globalization.CultureInfo.InvariantCulture, out var resultFloat))
					cell.SetCellValue(resultFloat);
				else
				{
					cell.SetCellValue(value);
				}
			}
		}

		var memoryStream = new MemoryStream();
		workbook.Write(memoryStream, leaveOpen: true);
		workbook.Close();
		memoryStream.Position = 0;

		return Task.FromResult(memoryStream);
	}
}