using System.Threading.Channels;
using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;

namespace LabTestBrowser.Infrastructure;

public class CompleteBloodCountUpdateChannel : ICompleteBloodCountUpdateNotifier, ICompleteBloodCountUpdateReader
{
	private readonly Channel<int> _channel = Channel.CreateUnbounded<int>();
	private int _readerAttached = 0;

	public async Task NotifyAsync(int completeBloodCountId)
	{
		await _channel.Writer.WriteAsync(completeBloodCountId);
	}

	public IAsyncEnumerable<int> ReadUpdatesAsync(CancellationToken cancellationToken = default)
	{
		if (Interlocked.Exchange(ref _readerAttached, 1) != 0)
			throw new InvalidOperationException("Only one consumer is allowed to read from the stream");

		return _channel.Reader.ReadAllAsync(cancellationToken);
	}
}