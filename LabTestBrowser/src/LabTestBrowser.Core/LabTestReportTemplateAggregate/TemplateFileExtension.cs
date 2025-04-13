namespace LabTestBrowser.Core.LabTestReportTemplateAggregate;

public class TemplateFileExtension : ValueObject
{
	private static readonly TemplateFileExtension Undefined = new(TemplateFileFormat.Undefined, string.Empty);
	private static readonly TemplateFileExtension Excel = new(TemplateFileFormat.Excel, "xlsx");
	private static readonly TemplateFileExtension Word = new(TemplateFileFormat.Word, "docx");

	private static readonly Dictionary<string, TemplateFileExtension> SupportedFileExtensions = new()
	{
		{
			"xlsx", Excel
		},
		{
			"xls", Excel
		},
		{
			"docx", Word
		},
		{
			"doc", Word
		}
	};

	private TemplateFileExtension(TemplateFileFormat fileFormat, string fileExtension)
	{
		FileFormat = fileFormat;
		FileExtension = fileExtension;
	}

	public TemplateFileFormat FileFormat { get; private set; }
	public string FileExtension { get; private set; }

	public static TemplateFileExtension Create(string path)
	{
		var fileExtension = Path.GetExtension(path);
		var normalizedFileExtension = fileExtension.ToLowerInvariant().TrimStart('.');
		return SupportedFileExtensions.GetValueOrDefault(normalizedFileExtension, Undefined);
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return FileFormat;
		yield return FileExtension;
	}
}