using LabTestBrowser.Core.LabTestReportTemplateAggregate;

namespace LabTestBrowser.UseCases.LabTestReportTemplates;

public record LabTestReportTemplateDto
{
	public required int Id { get; init; }
	public required string Title { get; init; }
	public required string Path { get; init; }
	public TemplateFileFormat FileFormat { get; init; }
	public required string FileExtension { get; init; }
}