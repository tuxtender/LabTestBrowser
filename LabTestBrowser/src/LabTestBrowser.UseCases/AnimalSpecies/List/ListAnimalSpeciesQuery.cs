namespace LabTestBrowser.UseCases.AnimalSpecies.List;

public record ListAnimalSpeciesQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<AnimalSpeciesDto>>>;