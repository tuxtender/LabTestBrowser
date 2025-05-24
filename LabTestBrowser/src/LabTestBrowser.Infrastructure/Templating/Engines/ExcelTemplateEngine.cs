using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LabTestBrowser.Infrastructure.Templating.Engines;

public class ExcelTemplateEngine : IExcelTemplateEngine
{
	private readonly ITextTemplateEngine _templateEngine;

	public ExcelTemplateEngine(ITextTemplateEngine templateEngine)
	{
		_templateEngine = templateEngine;
	}

	public Task<MemoryStream> RenderAsync(FileStream fileStream, IReadOnlyDictionary<string, string> tokens)
	{
		using var workbook = new XSSFWorkbook(fileStream);
		using var tempStream = new MemoryStream();

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

		workbook.Write(tempStream, leaveOpen: true);
		workbook.Close();
		tempStream.Position = 0;

		var memoryStream = new MemoryStream(tempStream.ToArray());
		return Task.FromResult(memoryStream);
	}
}