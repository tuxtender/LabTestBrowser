namespace LabTestBrowser.UseCases.LabTestReports.GetEmpty;

public record GetEmptyLabTestReportQuery(DateOnly OrderDate) : IQuery<Result<LabTestReportDto>>;