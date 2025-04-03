namespace LabTestBrowser.UseCases.LabTestReports.RemoveCompleteBloodCount;

public record RemoveCompleteBloodCountCommand(int LabTestReportId) : ICommand<Result>;