namespace LabTestBrowser.UseCases.LabTestReports.GetEmpty;

public record GetEmptyLabTestReportQuery(DateOnly Date) : IQuery<Result<LabTestReportDto>>;