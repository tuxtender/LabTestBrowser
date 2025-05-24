using LabTestBrowser.Infrastructure.Export;

namespace LabTestBrowser.Desktop.Localization;

public class ResxDefaultPathProvider : IDefaultPathProvider
{
	public string GetDefaultPath() => UI.Resources.Strings.Export_DefaultPath;
}