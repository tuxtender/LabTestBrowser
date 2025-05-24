namespace LabTestBrowser.UseCases.Export.Exceptions;

public class ExportFileAccessException : IOException
{
	public ExportFileAccessException(string filePath)
	{
		FilePath = filePath;
	}

	public ExportFileAccessException(string filePath, string message)
		: base(message)
	{
		FilePath = filePath;
	}

	public ExportFileAccessException(string filePath, string message, Exception inner)
		: base(message, inner)
	{
		FilePath = filePath;
	}

	public string FilePath { get; }
}