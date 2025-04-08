namespace LabTestBrowser.Infrastructure.Data.Settings;

public class LabReportTemplate
{
	public int AnimalId { get; init; }
	public required string LabTestTitle { get; init; }
	public required string TemplatePath { get; init; }
}