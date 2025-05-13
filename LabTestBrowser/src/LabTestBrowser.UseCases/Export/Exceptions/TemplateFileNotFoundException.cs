namespace LabTestBrowser.UseCases.Export.Exceptions;

public class TemplateFileNotFoundException : IOException
{
	public TemplateFileNotFoundException(string filePath)
	{
		FilePath = filePath;
	}

	public TemplateFileNotFoundException(string filePath, string message)
		: base(message)
	{
		FilePath = filePath;
	}

	public TemplateFileNotFoundException(string filePath, string message, Exception inner)
		: base(message, inner)
	{
		FilePath = filePath;
	}

	public string FilePath { get; }
}