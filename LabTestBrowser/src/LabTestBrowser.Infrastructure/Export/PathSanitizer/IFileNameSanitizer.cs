namespace LabTestBrowser.Infrastructure.Export.PathSanitizer;

public interface IFileNameSanitizer
{
	string Sanitize(string fileName);
}