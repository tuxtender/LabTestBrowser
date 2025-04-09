namespace LabTestBrowser.UseCases.LabTestReportTemplates;

public record LabTestReportExportFilename
{
	public required string Animal { get; init; }
	public required string LabTest { get; init; }
	public string? HealthcareProxy { get; init; }
	public string? Patient { get; init; }
	public string? Age { get; init; }
	public required string FileExtension { get; init; }
}