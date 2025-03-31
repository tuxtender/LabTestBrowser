using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Create;

public class CreateCompleteBloodCountHandler(IRepository<CompleteBloodCount> _repository, ICbcTestResultReader _reader) : ICommandHandler<CreateCompleteBloodCountCommand, Result<int>>
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
		
		var createdCbc = await _repository.AddAsync(cbc, cancellationToken);

		var cbcDto = new CompleteBloodCountDto
		{
			ExternalId = request.ExternalId,
			ObservationDateTime = request.ObservationDateTime,
			WhiteBloodCell = request.WhiteBloodCell,
			LymphocytePercent = request.LymphocytePercent,
			MonocytePercent = request.MonocytePercent,
			NeutrophilPercent = request.NeutrophilPercent,
			EosinophilPercent = request.EosinophilPercent,
			BasophilPercent = request.BasophilPercent,
			RedBloodCell = request.RedBloodCell,
			Hemoglobin = request.Hemoglobin,
			Hematocrit = request.Hematocrit,
			MeanCorpuscularVolume = request.MeanCorpuscularVolume,
			MeanCorpuscularHemoglobin = request.MeanCorpuscularHemoglobin,
			MeanCorpuscularHemoglobinConcentration = request.MeanCorpuscularHemoglobinConcentration,
			RedBloodCellDistributionWidth = request.RedBloodCellDistributionWidth,
			Platelet = request.Platelet,
			MeanPlateletVolume = request.MeanPlateletVolume,
		};
		
		await _reader.WriteAsync(cbcDto);
		
		return cbc.Id;
	}
}