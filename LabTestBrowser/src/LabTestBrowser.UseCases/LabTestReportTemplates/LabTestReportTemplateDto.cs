namespace LabTestBrowser.UseCases.LabTestReportTemplates;

public record LabTestReportTemplateDto
{
	public required string Title { get; init; }
	public required string Path { get; init; }
}