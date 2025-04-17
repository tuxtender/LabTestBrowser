namespace LabTestBrowser.UseCases.LabTestReports.GetPrevious;

public record GetPreviousLabTestReportQuery(int SequenceNumber, DateOnly Date) : IQuery<Result<LabTestReportDto>>;