using System.Threading.Channels;
using LabTestBrowser.Core.Interfaces;

namespace LabTestBrowser.Infrastructure;

public class CompleteBloodCountUpdateChannel : ICompleteBloodCountUpdateChannel
{
	private readonly Channel<int> _queue = Channel.CreateUnbounded<int>();

	public async Task<int> ReadAsync()
	{
		var completeBloodCountId = await _queue.Reader.ReadAsync().AsTask();
		return completeBloodCountId;
	}

	public async Task WriteAsync(int completeBloodCountId)
	{
		var writer = _queue.Writer;
		await writer.WriteAsync(completeBloodCountId);
	}
}