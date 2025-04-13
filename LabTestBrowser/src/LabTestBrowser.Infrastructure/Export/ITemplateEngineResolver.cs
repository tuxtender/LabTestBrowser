using LabTestBrowser.Core.LabTestReportTemplateAggregate;

namespace LabTestBrowser.Infrastructure.Export;

public interface ITemplateEngineResolver
{
	IFileTemplateEngine ResolveByFileFormat(TemplateFileExtension templateFileFormat);
}