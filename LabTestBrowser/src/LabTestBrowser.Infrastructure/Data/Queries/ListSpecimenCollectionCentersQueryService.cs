using LabTestBrowser.UseCases.SpecimenCollectionCenters;

namespace LabTestBrowser.Infrastructure.Data.Queries;

public class ListSpecimenCollectionCentersQueryService(IEnumerable<SpecimenCollectionCenterDto> specimenCollectionCenters)
	: IListSpecimenCollectionCentersQueryService
{
	private readonly IEnumerable<SpecimenCollectionCenterDto> _specimenCollectionCenters = specimenCollectionCenters;

	public Task<IEnumerable<SpecimenCollectionCenterDto>> ListAsync()
	{
		return Task.FromResult(_specimenCollectionCenters);
	}
}