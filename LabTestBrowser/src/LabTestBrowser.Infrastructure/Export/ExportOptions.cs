namespace LabTestBrowser.Infrastructure.Export;

public class ExportOptions
{
	public const string SectionName = "ExportSettings";
	public required string Directory { get; init; }
	public required string Filename { get; init; }
}