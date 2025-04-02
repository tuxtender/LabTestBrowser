using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.UseCases.CompleteBloodCounts.Get;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.GetCreated;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetCreatedCompleteBloodCountHandler(IReadRepository<CompleteBloodCount> _repository, ICbcTestResultReader _reader)
	: IQueryHandler<GetCreatedCompleteBloodCountQuery, Result<CompleteBloodCountDto>>
{
	private readonly IReadRepository<CompleteBloodCount> _repository = _repository;

	public async Task<Result<CompleteBloodCountDto>> Handle(GetCreatedCompleteBloodCountQuery request, CancellationToken cancellationToken)
	{
		// var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId, cancellationToken);

		var cbcDto = await _reader.ReadAsync();
		
		return cbcDto;
		
		// return new CompleteBloodCountDto
		// {
		// 	ExternalId = cbc!.ExternalId,
		// 	ObservationDateTime = cbc.ObservationDateTime,
		// 	WhiteBloodCell = cbc.WhiteBloodCell!.Value,
		// 	LymphocytePercent = cbc.LymphocytePercent!.Value,
		// 	MonocytePercent = cbc.MonocytePercent!.Value,
		// 	NeutrophilPercent = cbc.NeutrophilPercent!.Value,
		// 	EosinophilPercent = cbc.EosinophilPercent!.Value,
		// 	BasophilPercent = cbc.BasophilPercent!.Value,
		// 	RedBloodCell = cbc.RedBloodCell!.Value,
		// 	Hemoglobin = cbc.Hemoglobin!.Value,
		// 	Hematocrit = cbc.Hematocrit!.Value,
		// 	MeanCorpuscularVolume = cbc.MeanCorpuscularVolume!.Value,
		// 	MeanCorpuscularHemoglobin = cbc.MeanCorpuscularHemoglobin!.Value,
		// 	MeanCorpuscularHemoglobinConcentration = cbc.MeanCorpuscularHemoglobinConcentration!.Value,
		// 	RedBloodCellDistributionWidth = cbc.RedBloodCellDistributionWidth!.Value,
		// 	Platelet = cbc.Platelet!.Value,
		// 	MeanPlateletVolume = cbc.MeanPlateletVolume!.Value,
		// };
	}
}