namespace LabTestBrowser.UseCases.LabTestReports.GetLast;

public record GetLastLabTestReportQuery(DateOnly OrderDate) : IQuery<Result<LabTestReportDto>>;