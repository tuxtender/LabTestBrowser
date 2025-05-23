namespace LabTestBrowser.UseCases.LabTestReports.GetNext;

public record GetNextLabTestReportQuery(int OrderNumber, DateOnly OrderDate) : IQuery<Result<LabTestReportDto>>;