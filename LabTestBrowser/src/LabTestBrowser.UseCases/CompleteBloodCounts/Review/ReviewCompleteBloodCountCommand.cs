namespace LabTestBrowser.UseCases.CompleteBloodCounts.Review;

public record ReviewCompleteBloodCountCommand(int? CompleteBloodCountId, int SequenceNumber, DateOnly Date) : ICommand<Result>;