namespace LabTestBrowser.Infrastructure.Export.PathSanitizer;

public interface IDirectoryNameSanitizer
{
	string Sanitize(string directoryName);
}