namespace LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;

public interface ICompleteBloodCountUpdateReader
{
	IAsyncEnumerable<int> ReadUpdatesAsync(CancellationToken cancellationToken = default);
}