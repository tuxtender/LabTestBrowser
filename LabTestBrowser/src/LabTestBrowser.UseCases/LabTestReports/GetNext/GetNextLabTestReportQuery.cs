namespace LabTestBrowser.UseCases.LabTestReports.GetNext;

public record GetNextLabTestReportQuery(int SequenceNumber, DateOnly Date) : IQuery<Result<LabTestReportDto>>;