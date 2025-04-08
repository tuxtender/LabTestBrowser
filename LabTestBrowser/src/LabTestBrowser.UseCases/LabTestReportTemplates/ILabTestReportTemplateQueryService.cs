using LabTestBrowser.Core.LabTestReportTemplateAggregate;

namespace LabTestBrowser.UseCases.LabTestReportTemplates;

public interface ILabTestReportTemplateQueryService
{
	Task<List<LabTestReportTemplateDto>> GetLabTestReportTemplatesAsync(string facility, string tradeName, int animalId);
}