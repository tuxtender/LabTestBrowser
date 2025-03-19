namespace LabTestBrowser.UseCases.LabTestReports.GetNext;

public record GetNextLabTestReportQuery(int LabTestReportId) : IQuery<Result<LabTestReportDTO>>;