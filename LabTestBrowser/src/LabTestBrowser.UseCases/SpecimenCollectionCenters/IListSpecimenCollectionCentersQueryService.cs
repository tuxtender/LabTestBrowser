namespace LabTestBrowser.UseCases.SpecimenCollectionCenters;

public interface IListSpecimenCollectionCentersQueryService
{
	Task<IEnumerable<SpecimenCollectionCenterDto>> ListAsync();
}