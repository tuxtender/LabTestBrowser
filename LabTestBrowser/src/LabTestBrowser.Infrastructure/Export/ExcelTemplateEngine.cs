using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LabTestBrowser.Infrastructure.Export;

public class ExcelTemplateEngine : IExcelTemplateEngine
{
	private readonly ITextTemplateEngine _templateEngine;

	public ExcelTemplateEngine(ITextTemplateEngine templateEngine)
	{
		_templateEngine = templateEngine;
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

				if (string.IsNullOrEmpty(cellValue))
					continue;

				var value = _templateEngine.Render(cellValue, tokens);

				if (float.TryParse(value, System.Globalization.CultureInfo.InvariantCulture, out var numericValue))
					cell.SetCellValue(numericValue);
				else
					cell.SetCellValue(value);
			}
		}

		var memoryStream = new MemoryStream();
		workbook.Write(memoryStream, leaveOpen: true);
		workbook.Close();
		memoryStream.Position = 0;

		return Task.FromResult(memoryStream);
	}
}