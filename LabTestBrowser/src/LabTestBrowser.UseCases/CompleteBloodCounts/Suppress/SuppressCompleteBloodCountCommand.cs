namespace LabTestBrowser.UseCases.CompleteBloodCounts.Suppress;

public record SuppressCompleteBloodCountCommand(int? CompleteBloodCountId, DateOnly SuppressionDate) : ICommand<Result>;