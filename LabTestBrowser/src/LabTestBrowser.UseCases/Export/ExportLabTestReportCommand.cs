namespace LabTestBrowser.UseCases.Export;

public record ExportLabTestReportCommand(int? LabTestReportId, IEnumerable<int> LabTestReportTemplateIds) : ICommand<Result>;