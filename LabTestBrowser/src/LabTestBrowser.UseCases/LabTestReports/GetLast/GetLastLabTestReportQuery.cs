namespace LabTestBrowser.UseCases.LabTestReports.GetLast;

public record GetLastLabTestReportQuery(DateOnly Date) : IQuery<Result<LabTestReportDto>>;