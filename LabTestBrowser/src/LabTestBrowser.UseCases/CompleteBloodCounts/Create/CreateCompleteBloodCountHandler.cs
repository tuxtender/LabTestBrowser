using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Create;

public class CreateCompleteBloodCountHandler(IRepository<CompleteBloodCount> _repository) : ICommandHandler<CreateCompleteBloodCountCommand, Result<int>>
{
	public async Task<Result<int>> Handle(CreateCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		var cbc = new CompleteBloodCount(request.ExternalId, request.ObservationDateTime);
		
		cbc.SetWhiteBloodCell(request.WhiteBloodCell);
		cbc.SetLymphocyte(request.Lymphocyte);
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
		cbc.SetMeanPlateletVolume(request.Platelet);
		cbc.SetMeanPlateletVolume(request.MeanPlateletVolume);
		
		var createdCbc = await _repository.AddAsync(cbc, cancellationToken);

		return cbc.Id;
	}
}