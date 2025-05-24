namespace LabTestBrowser.UseCases.Export.Exceptions;

public class TemplateFileAccessException : IOException
{
	public TemplateFileAccessException(string filePath)
	{
		FilePath = filePath;
	}

	public TemplateFileAccessException(string filePath, string message)
		: base(message)
	{
		FilePath = filePath;
	}

	public TemplateFileAccessException(string filePath, string message, Exception inner)
		: base(message, inner)
	{
		FilePath = filePath;
	}

	public string FilePath { get; }
}