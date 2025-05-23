namespace LabTestBrowser.UseCases.CompleteBloodCounts.Review;

public record ReviewCompleteBloodCountCommand(int? CompleteBloodCountId, int LabOrderNumber, DateOnly LabOrderDate) : ICommand<Result>;