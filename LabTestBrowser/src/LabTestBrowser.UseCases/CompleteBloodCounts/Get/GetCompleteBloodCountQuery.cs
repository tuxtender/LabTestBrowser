namespace LabTestBrowser.UseCases.CompleteBloodCounts.Get;

public record GetCompleteBloodCountQuery(int CompleteBloodCountId) : IQuery<Result<CompleteBloodCountDto>>;