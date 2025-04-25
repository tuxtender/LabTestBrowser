using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Create;

public class CreateCompleteBloodCountHandler(
	IRepository<CompleteBloodCount> _repository,
	ICompleteBloodCountUpdateChannel _updateChannel,
	ILogger<CreateCompleteBloodCountHandler> _logger) : ICommandHandler<CreateCompleteBloodCountCommand, Result<int>>
{
	public async Task<Result<int>> Handle(CreateCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		var cbc = new CompleteBloodCount(request.ExternalId, request.ObservationDateTime);
		cbc.SetWhiteBloodCell(request.WhiteBloodCell);
		cbc.SetLymphocytePercent(request.LymphocytePercent);
		cbc.SetMonocytePercent(request.MonocytePercent);
		cbc.SetNeutrophilPercent(request.NeutrophilPercent);
		cbc.SetEosinophilPercent(request.EosinophilPercent);
		cbc.SetBasophilPercent(request.BasophilPercent);
		cbc.SetRedBloodCell(request.RedBloodCell);
		cbc.SetHemoglobin(request.Hemoglobin);
		cbc.SetHematocrit(request.Hematocrit);
		cbc.SetMeanCorpuscularVolume(request.MeanCorpuscularVolume);
		cbc.SetMeanCorpuscularHemoglobin(request.MeanCorpuscularHemoglobin);
		cbc.SetMeanCorpuscularHemoglobinConcentration(request.MeanCorpuscularHemoglobinConcentration);
		cbc.SetRedBloodCellDistributionWidth(request.RedBloodCellDistributionWidth);
		cbc.SetPlatelet(request.Platelet);
		cbc.SetMeanPlateletVolume(request.MeanPlateletVolume);

		await _repository.AddAsync(cbc, cancellationToken);
		await _updateChannel.WriteAsync(cbc.Id);

		_logger.LogInformation("Created complete blood count id: {completeBloodCountId}", cbc.Id);

		return cbc.Id;
	}
}