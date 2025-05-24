namespace LabTestBrowser.Infrastructure.Export;

public class BasePathProvider : IBasePathProvider
{
	public string GetBasePath() => Directory.GetCurrentDirectory();
}