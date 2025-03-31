using System.Threading.Channels;
using LabTestBrowser.UseCases.CompleteBloodCounts;

namespace LabTestBrowser.Infrastructure;

public class CbcTestResultReader : ICbcTestResultReader
{
	private readonly Channel<CompleteBloodCountDto> _queue = Channel.CreateUnbounded<CompleteBloodCountDto>();

	public async Task<CompleteBloodCountDto> ReadAsync()
	{
		var cbcTestResults = await _queue.Reader.ReadAsync().AsTask();

		return cbcTestResults;
	}

	public async Task WriteAsync(CompleteBloodCountDto testResults)
	{
		// Код-производитель
		var writer = _queue.Writer;
		await writer.WriteAsync(testResults);

		// writer.Complete();
	}
}