namespace LabTestBrowser.UnitTests.Infrastructure.Templating.Engines;

public record TemplateTestCase
{
	public required string Template { get; init; }
	public required string Expected { get; init; }
	public string[] Keys { get; init; } = [];
	public string[] Values { get; init; } = [];
}