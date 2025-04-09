namespace LabTestBrowser.UseCases.LabTestReportTemplates.List;

public record ListLabTestReportTemplatesQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<LabTestReportTemplateDto>>>;