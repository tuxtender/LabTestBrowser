namespace LabTestBrowser.UseCases.LabTestReportTemplates.ListRegistered;

public record ListRegisteredLabTestReportTemplatesQuery(int LabTestReportId) : IQuery<Result<IEnumerable<LabTestReportTemplateDto>>>;