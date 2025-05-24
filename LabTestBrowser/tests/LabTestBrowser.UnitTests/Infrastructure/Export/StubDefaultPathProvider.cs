using LabTestBrowser.Infrastructure.Export;

namespace LabTestBrowser.UnitTests.Infrastructure.Export;

public class StubDefaultPathProvider(string value) : IDefaultPathProvider
{
	public string GetDefaultPath() => value;
}