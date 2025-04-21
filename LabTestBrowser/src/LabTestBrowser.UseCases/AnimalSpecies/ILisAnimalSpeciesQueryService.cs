namespace LabTestBrowser.UseCases.AnimalSpecies;

public interface ILisAnimalSpeciesQueryService
{
	Task<IEnumerable<AnimalSpeciesDto>> ListAsync();
}