namespace LabTestBrowser.UseCases.AnimalSpecies.List;

public class ListAnimalSpeciesHandler(ILisAnimalSpeciesQueryService _queryService)
	: IQueryHandler<ListAnimalSpeciesQuery,
		Result<IEnumerable<AnimalSpeciesDto>>>
{
	public async Task<Result<IEnumerable<AnimalSpeciesDto>>> Handle(ListAnimalSpeciesQuery request,
		CancellationToken cancellationToken)
	{
		var centers = await _queryService.ListAsync();

		return Result.Success(centers);
	}
}