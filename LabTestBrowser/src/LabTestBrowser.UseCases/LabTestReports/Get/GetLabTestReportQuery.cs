namespace LabTestBrowser.UseCases.LabTestReports.Get;

public record GetLabTestReportQuery(int? LabOrderNumber, DateOnly? LabOrderDate) : IQuery<Result<LabTestReportDto>>;