namespace LabTestBrowser.UseCases.LabTestReports.Export;

public record ExportLabTestReportCommand(int Specimen, DateOnly Date, IEnumerable<int> LabTestReportTemplateIds) : ICommand<Result>;