namespace LabTestBrowser.UseCases.LabTestReports.GetPrevious;

public record GetPreviousLabTestReportQuery(int Specimen, DateOnly Date) : IQuery<Result<LabTestReportDto>>;