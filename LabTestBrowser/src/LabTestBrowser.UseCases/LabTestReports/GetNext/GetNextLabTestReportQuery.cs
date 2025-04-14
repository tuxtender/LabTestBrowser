namespace LabTestBrowser.UseCases.LabTestReports.GetNext;

public record GetNextLabTestReportQuery(int Specimen, DateOnly Date) : IQuery<Result<LabTestReportDto>>;