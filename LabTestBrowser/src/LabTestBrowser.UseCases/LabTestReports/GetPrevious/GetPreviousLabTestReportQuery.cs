namespace LabTestBrowser.UseCases.LabTestReports.GetPrevious;

public record GetPreviousLabTestReportQuery(int OrderNumber, DateOnly OrderDate) : IQuery<Result<LabTestReportDto>>;