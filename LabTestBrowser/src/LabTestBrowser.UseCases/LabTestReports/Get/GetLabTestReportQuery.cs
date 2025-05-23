namespace LabTestBrowser.UseCases.LabTestReports.Get;

public record GetLabTestReportQuery(int OrderNumber, DateOnly OrderDate) : IQuery<Result<LabTestReportDto>>;