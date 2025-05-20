namespace LabTestBrowser.Infrastructure.Export.PathSanitizer;

public interface IPathSanitizer
{
	string Sanitize(string pathComponent);
}