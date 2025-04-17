namespace LabTestBrowser.UseCases.CompleteBloodCounts.ResetReview;

public record ResetCompleteBloodCountCommand(int? CompleteBloodCountId) : ICommand<Result>;