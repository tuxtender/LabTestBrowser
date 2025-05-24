namespace LabTestBrowser.UseCases.SpecimenCollectionCenters.List;

public class ListSpecimenCollectionCentersHandler(IListSpecimenCollectionCentersQueryService _queryService)
	: IQueryHandler<ListSpecimenCollectionCentersQuery,
		Result<IEnumerable<SpecimenCollectionCenterDto>>>
{
	public async Task<Result<IEnumerable<SpecimenCollectionCenterDto>>> Handle(ListSpecimenCollectionCentersQuery request,
		CancellationToken cancellationToken)
	{
		var centers = await _queryService.ListAsync();

		return Result.Success(centers);
	}
}