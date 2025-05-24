using LabTestBrowser.Core.LabTestReportTemplateAggregate;

namespace LabTestBrowser.UseCases.LabTestReportTemplates;

public interface ILabTestReportTemplateQueryService
{
	Task<LabTestReportTemplate?> FindById(int labTestReportTemplateId);
	Task<IEnumerable<LabTestReportTemplate>> ListAsync(string facility, string? tradeName, string animal);
	Task<IEnumerable<LabTestReportTemplate>> ListAsync();
}