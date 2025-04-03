using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Get;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetCompleteBloodCountHandler(IReadRepository<CompleteBloodCount> _repository)
	: IQueryHandler<GetCompleteBloodCountQuery, Result<CompleteBloodCountDto>>
{
	public async Task<Result<CompleteBloodCountDto>> Handle(GetCompleteBloodCountQuery request, CancellationToken cancellationToken)
	{
		var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId, cancellationToken);

		if (cbc == null)
			return Result.NotFound();

		return new CompleteBloodCountDto
		{
			Id = cbc.Id,
			ExternalId = cbc!.ExternalId,
			ObservationDateTime = cbc.ObservationDateTime,
			WhiteBloodCell = cbc.WhiteBloodCell!.Value,
			LymphocytePercent = cbc.LymphocytePercent!.Value,
			MonocytePercent = cbc.MonocytePercent!.Value,
			NeutrophilPercent = cbc.NeutrophilPercent!.Value,
			EosinophilPercent = cbc.EosinophilPercent!.Value,
			BasophilPercent = cbc.BasophilPercent!.Value,
			RedBloodCell = cbc.RedBloodCell!.Value,
			Hemoglobin = cbc.Hemoglobin!.Value,
			Hematocrit = cbc.Hematocrit!.Value,
			MeanCorpuscularVolume = cbc.MeanCorpuscularVolume!.Value,
			MeanCorpuscularHemoglobin = cbc.MeanCorpuscularHemoglobin!.Value,
			MeanCorpuscularHemoglobinConcentration = cbc.MeanCorpuscularHemoglobinConcentration!.Value,
			RedBloodCellDistributionWidth = cbc.RedBloodCellDistributionWidth!.Value,
			Platelet = cbc.Platelet!.Value,
			MeanPlateletVolume = cbc.MeanPlateletVolume!.Value,
		};
	}
}