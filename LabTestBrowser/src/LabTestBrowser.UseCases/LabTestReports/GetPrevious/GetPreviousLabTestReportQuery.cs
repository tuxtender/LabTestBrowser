namespace LabTestBrowser.UseCases.LabTestReports.GetPrevious;

public record GetPreviousLabTestReportQuery(int LabTestReportId) : IQuery<Result<LabTestReportDto>>;