namespace LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;

public interface IGetUpdatedCompleteBloodCountsUseCase
{
	IAsyncEnumerable<CompleteBloodCountDto> ExecuteAsync(CancellationToken cancellationToken = default);
}