using LabTestBrowser.Core.LabTestReportTemplateAggregate;

namespace LabTestBrowser.UseCases.LabTestReportTemplates;

public static class LabTestReportTemplateExtensions
{
	public static LabTestReportTemplateDto ConvertToLabTestReportTemplateDto(this LabTestReportTemplate template)
	{
		return new LabTestReportTemplateDto
		{
			Id = template.Id,
			Title = template.Title,
			Path = template.Path,
			FileFormat = template.TemplateFileExtension.FileFormat,
			FileExtension = template.TemplateFileExtension.FileExtension
		};
	}
}