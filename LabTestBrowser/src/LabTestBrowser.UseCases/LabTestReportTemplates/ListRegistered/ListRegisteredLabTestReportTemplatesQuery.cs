namespace LabTestBrowser.UseCases.LabTestReportTemplates.ListRegistered;

public record ListRegisteredLabTestReportTemplatesQuery(int Specimen, DateOnly Date) : IQuery<Result<IEnumerable<LabTestReportTemplateDto>>>;