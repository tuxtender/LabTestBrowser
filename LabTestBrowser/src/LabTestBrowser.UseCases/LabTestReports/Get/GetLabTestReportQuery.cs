namespace LabTestBrowser.UseCases.LabTestReports.Get;

public record GetLabTestReportQuery(int SequenceNumber, DateOnly Date) : IQuery<Result<LabTestReportDto>>;