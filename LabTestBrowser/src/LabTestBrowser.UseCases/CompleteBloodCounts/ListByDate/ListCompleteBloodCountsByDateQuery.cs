namespace LabTestBrowser.UseCases.CompleteBloodCounts.ListByDate;

public record ListCompleteBloodCountsByDateQuery(DateOnly Date) : IQuery<Result<IEnumerable<CompleteBloodCountDto>>>;