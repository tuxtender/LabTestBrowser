namespace LabTestBrowser.UseCases.LabTestReports.Export;

public record ExportLabTestReportCommand(int? LabTestReportId, IEnumerable<int> LabTestReportTemplateIds) : ICommand<Result>;