using LabTestBrowser.Infrastructure.Export;

namespace LabTestBrowser.UI.Localization;

public class ResxDefaultPathProvider : IDefaultPathProvider
{
	public string GetDefaultPath() => Resources.Strings.Export_DefaultPath;
}