using System.Runtime.CompilerServices;
using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;

public class GetUpdatedCompleteBloodCountsUseCase : IGetUpdatedCompleteBloodCountsUseCase
{
	private readonly IReadRepository<CompleteBloodCount> _repository;
	private readonly ICompleteBloodCountUpdateChannel _updateChannel;
	private readonly ILogger<GetUpdatedCompleteBloodCountsUseCase> _logger;

	public GetUpdatedCompleteBloodCountsUseCase(IReadRepository<CompleteBloodCount> repository,
		ICompleteBloodCountUpdateChannel updateChannel,
		ILogger<GetUpdatedCompleteBloodCountsUseCase> logger)
	{
		_repository = repository;
		_updateChannel = updateChannel;
		_logger = logger;
	}

	public async IAsyncEnumerable<CompleteBloodCountDto> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
	{
		_logger.LogInformation("Complete blood count stream started");

		while (!cancellationToken.IsCancellationRequested)
		{
			var completeBloodCountId = await _updateChannel.ReadAsync();
			var cbc = await _repository.GetByIdAsync(completeBloodCountId, cancellationToken);
			if (cbc == null)
			{
				_logger.LogWarning("Inconsistent data. Complete blood count id: {completeBloodCountId} hasn't been saved yet",
					completeBloodCountId);
				continue;
			}

			yield return cbc.ConvertToCompleteBloodCountDto();
			_logger.LogDebug("Complete blood count id: {completeBloodCountId} yielded into the sequence", completeBloodCountId);
		}

		_logger.LogInformation("Complete blood count stream canceled");
	}
}