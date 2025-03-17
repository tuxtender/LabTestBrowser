namespace LabTestBrowser.UseCases.LabTestReports.GetAvailable;

public record GetAvailableLabTestReportQuery(DateOnly Date) : IQuery<Result<LabTestReportDTO>>;