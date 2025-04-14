namespace LabTestBrowser.UseCases.LabTestReports.Get;

public record GetLabTestReportQuery(int LabTestReportId) : IQuery<Result<LabTestReportDto>>;