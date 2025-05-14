using LabTestBrowser.Core.LabTestReportTemplateAggregate;

namespace LabTestBrowser.Infrastructure.Templating.Engines;

public interface ITemplateEngineResolver
{
	IFileTemplateEngine ResolveByFileFormat(TemplateFileFormat templateFileFormat);
}