using System.Runtime.CompilerServices;
using LabTestBrowser.Core.CompleteBloodCountAggregate;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;

public class GetUpdatedCompleteBloodCountsUseCase : IGetUpdatedCompleteBloodCountsUseCase
{
	private readonly IReadRepository<CompleteBloodCount> _repository;
	private readonly ICompleteBloodCountUpdateReader _reader;
	private readonly ILogger<GetUpdatedCompleteBloodCountsUseCase> _logger;

	public GetUpdatedCompleteBloodCountsUseCase(IReadRepository<CompleteBloodCount> repository,
		ICompleteBloodCountUpdateReader reader,
		ILogger<GetUpdatedCompleteBloodCountsUseCase> logger)
	{
		_repository = repository;
		_reader = reader;
		_logger = logger;
	}

	public async IAsyncEnumerable<CompleteBloodCountDto> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
	{
		_logger.LogInformation("Complete blood count stream started");

		await foreach (var id in _reader.ReadUpdatesAsync(cancellationToken))
		{
			var cbc = await _repository.GetByIdAsync(id, cancellationToken);
			if (cbc == null)
			{
				_logger.LogWarning("Inconsistent data. Complete blood count id: {CompleteBloodCountId} hasn't been saved yet", id);
				continue;
			}

			yield return cbc.ConvertToCompleteBloodCountDto();
		}

		_logger.LogInformation("Complete blood count stream canceled");
	}
}