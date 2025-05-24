using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportTemplateAggregate;

namespace LabTestBrowser.UseCases.Export;

public interface IExportService
{
	Task ExportAsync(LabTestReportTemplate reportTemplate, LabTestReport report, CompleteBloodCount? completeBloodCount);
}