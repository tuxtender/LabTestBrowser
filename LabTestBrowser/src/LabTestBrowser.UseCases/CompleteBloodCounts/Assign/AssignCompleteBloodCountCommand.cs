namespace LabTestBrowser.UseCases.CompleteBloodCounts.Assign;

public record AssignCompleteBloodCountCommand(int? CompleteBloodCountId, int SequenceNumber, DateOnly Date) : ICommand<Result>;