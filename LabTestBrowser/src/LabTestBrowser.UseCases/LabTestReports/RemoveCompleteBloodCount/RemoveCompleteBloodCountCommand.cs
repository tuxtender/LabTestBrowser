namespace LabTestBrowser.UseCases.LabTestReports.RemoveCompleteBloodCount;

public record RemoveCompleteBloodCountCommand(int Specimen, DateOnly Date) : ICommand<Result>;