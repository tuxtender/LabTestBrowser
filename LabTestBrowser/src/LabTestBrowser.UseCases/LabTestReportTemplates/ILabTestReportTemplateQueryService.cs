namespace LabTestBrowser.UseCases.LabTestReportTemplates;

public interface ILabTestReportTemplateQueryService
{
	Task<LabTestReportTemplateDto?> FindById(int labTestReportTemplateId);
	Task<IEnumerable<LabTestReportTemplateDto>> ListAsync(string facility, string? tradeName, string animal);
	Task<IEnumerable<LabTestReportTemplateDto>> ListAsync();
}