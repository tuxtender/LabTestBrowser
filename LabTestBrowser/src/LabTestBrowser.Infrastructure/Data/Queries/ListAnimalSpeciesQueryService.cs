using LabTestBrowser.UseCases.AnimalSpecies;

namespace LabTestBrowser.Infrastructure.Data.Queries;

public class ListAnimalSpeciesQueryService(IEnumerable<AnimalSpeciesDto> animalPatients) : ILisAnimalSpeciesQueryService
{
	private readonly IEnumerable<AnimalSpeciesDto> _animalPatients = animalPatients;

	public Task<IEnumerable<AnimalSpeciesDto>> ListAsync()
	{
		return Task.FromResult(_animalPatients);
	}
}